using AutoMapper;
using BL.DTO.User;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager,
            IMapper mapper) {
            this._userManager = userManager;    
            this._mapper = mapper;
        }
        public async Task<GetUserPorfileDTO> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);    

            if(user == null) 
                throw new DataNotFoundException("User not found");

            var userProfileDTO = _mapper.Map<GetUserPorfileDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userProfileDTO.Role = String.Join(",", roles);

            return userProfileDTO;
        }
        public async Task<GetUserPorfileDTO> UpdateProfileAsync(UpdateUserProfileDTO profileDTO, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new DataNotFoundException("User not found");

            user.FirstName = profileDTO.FirstName ?? user.FirstName;
            user.SecondName = profileDTO.SecondName ?? user.SecondName;
            user.ThirdName = profileDTO.ThirdName ?? user.ThirdName;
            user.LastName = profileDTO.LastName ?? user.LastName;
            user.PhoneNumber = profileDTO.PhoneNumber ?? user.PhoneNumber;
            user.CityId = profileDTO.CityId ?? user.CityId;

            try
            {
                var result = await _userManager.UpdateAsync(user);


                if (!result.Succeeded)
                {
                    var identityErrors = result.Errors.ToList();

                    var fields = identityErrors
                        .GroupBy(e => FieldMapper.MapField(e.Code))
                        .ToDictionary(
                            g => g.Key,
                            g => g.First().Description
                        );

                    bool isDuplicate = identityErrors.Any(e =>
                        e.Code == "DuplicateEmail" ||
                        e.Code == "DuplicateUserName"
                    );

                    if (isDuplicate)
                    {
                        throw new ConflictException(
                            "Duplicate resource",
                            fields
                        );
                    }

                    throw new ValidationException(
                        "Validation failed",
                        fields
                    );
                }
                var userProfileDTO = _mapper.Map<GetUserPorfileDTO>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userProfileDTO.Role = String.Join(",", roles);

                return userProfileDTO;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                throw new ConflictException("Duplicate resource", new { PhoneNumber = "Phone Number is already taken." });
            }
        }
    }
}
