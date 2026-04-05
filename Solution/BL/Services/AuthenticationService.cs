using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTO.User;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private IConfiguration _config { get; }

        public AuthenticationService(UserManager<AppUser> userManager,
            IJwtService jwtService, IConfiguration config,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _config = config;
            _mapper = mapper;
        }
        public async Task<ReturnRegisteredUserDTO> Regsiter(AddUserDTO userDTO, string RoleName)
        {
            var user = _mapper.Map<AppUser>(userDTO);

            try
            {
                var result = await _userManager.CreateAsync(user, userDTO.Password);

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

            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                throw new ConflictException("Duplicate resource", new { PhoneNumber = "Phone Number is already taken." });
            }


            await _userManager.AddToRoleAsync(user, RoleName);

            var returnUser = _mapper.Map<ReturnRegisteredUserDTO>(user);
            returnUser.Role = RoleName;

            return returnUser;
        }
    }
}
