using AutoMapper;
using BL.DTO.User;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.DBContext;
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
        private readonly AlMarsadDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DTOBuilder _dtoBuilder;

        public UserService(UserManager<AppUser> userManager,
            IMapper mapper,
            AlMarsadDbContext dbContext,
             DTOBuilder dtoBuilder) {
            this._userManager = userManager;    
            this._mapper = mapper;
            this._dbContext = dbContext;
            this._dtoBuilder = dtoBuilder;
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
        public async Task<GetUserPorfileDTO> UpdateProfileAsync(UpdateUserProfileDTO dto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new DataNotFoundException("User not found");

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.SecondName = dto.SecondName ?? user.SecondName;
            user.ThirdName = dto.ThirdName ?? user.ThirdName;
            user.LastName = dto.LastName ?? user.LastName;

            if (!string.IsNullOrEmpty(dto.PhoneNumber) && user.PhoneNumber != dto.PhoneNumber)
                user.PhoneNumber = dto.PhoneNumber;

            user.CityId = dto.CityId ?? user.CityId;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                IdentityHandler.HandleIdentityErrors(result);

            return await _dtoBuilder.BuildUserProfileDTO(user);
        }

        public async Task<GetUserPorfileDTO> AdminUpdateUserAsync(UpdateFullUserAccountDTO dto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new DataNotFoundException("User not found");

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                user.FirstName = dto.FirstName ?? user.FirstName;
                user.SecondName = dto.SecondName ?? user.SecondName;
                user.ThirdName = dto.ThirdName ?? user.ThirdName;
                user.LastName = dto.LastName ?? user.LastName;

                if (!string.IsNullOrEmpty(dto.PhoneNumber) && user.PhoneNumber != dto.PhoneNumber)
                    user.PhoneNumber = dto.PhoneNumber;
                
                user.CityId = dto.CityId ?? user.CityId;

                if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(dto.Email);

                    if (existingUser != null && existingUser.Id != user.Id)
                    {
                        throw new ConflictException("Duplicate resource", new
                        {
                            Email = "Email is already taken"
                        });
                    }

                    user.Email = dto.Email;
                }

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    IdentityHandler.HandleIdentityErrors(result);

                if (dto.ResetPassword == true)
                {
                    if (string.IsNullOrWhiteSpace(dto.Password))
                        throw new ValidationException("Password is required when ResetPassword is true");

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);

                    if (!resetResult.Succeeded)
                        IdentityHandler.HandleIdentityErrors(resetResult);

                    user.RefreshToken = null;
                    user.RefreshTokenExpirationTime = null;

                    await _userManager.UpdateSecurityStampAsync(user);

                    var result2 = await _userManager.UpdateAsync(user);
                    if (!result2.Succeeded)
                        IdentityHandler.HandleIdentityErrors(result2);
                }

                await transaction.CommitAsync();

                return await _dtoBuilder.BuildUserProfileDTO(user);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordDTO passwordDTO, string userId)
        {
            if (passwordDTO.CurrentPassword == passwordDTO.NewPassword)
            {
                throw new ValidationException("Validation failed", new
                {
                    NewPassword = "New password must be different from current password"
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new DataNotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(user, passwordDTO.CurrentPassword, passwordDTO.NewPassword);

            if (!result.Succeeded)
                IdentityHandler.HandleIdentityErrors(result);
        }

    }
}
