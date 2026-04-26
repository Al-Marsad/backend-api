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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private IConfiguration _config { get; }

        public AuthenticationService(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IJwtService jwtService, IConfiguration config,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _config = config;
            _mapper = mapper;
            _signInManager = signInManager;
        }
        public async Task<ReturnRegisteredUserDTO> Regsiter(AddUserDTO userDTO, string RoleName)
        {
            var user = _mapper.Map<AppUser>(userDTO);

            try
            {
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                    IdentityHandler.HandleIdentityErrors(result);

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
        public async Task<ReturnLoginUserDTO> Login(LoginUserDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                throw new UnauthorizedException("There is no user with this email");

            };

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                var lockoutEnd = user.LockoutEnd;
                throw new UnauthorizedException($"Account locked. Please try again after {lockoutEnd?.UtcDateTime:t} UTC.");
            }

            if (!result.Succeeded)
            {
                throw new UnauthorizedException("Password is incorrect");
            }
            

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:ExpiryInDays"] ?? "7"));

            var result2 = await _userManager.UpdateAsync(user);

            if (!result2.Succeeded)
            {
                throw new Exception();
            };

            var returnUser = _mapper.Map<AppUser, ReturnLoginUserDTO>(user);
            returnUser.Role = String.Join(",", roles);
            returnUser.RefreshToken = refreshToken;
            returnUser.AccessToken = accessToken;
            returnUser.ExpiresIn = Convert.ToInt32(_config["Jwt:ExpiryInMinutes"] ?? "5");

            return returnUser;
        }

        public async Task<ReturnAccessTokenDTO> Refresh(string refreshToken)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshToken == refreshToken);
            
            if (user == null)
            {
                throw new UnauthorizedException("There is no user with this refresh token");
            }
            
            if(user.RefreshTokenExpirationTime <= DateTime.Now)
            {
                throw new UnauthorizedException("Refresh token is expired");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken = _jwtService.GenerateAccessToken(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var newRefreshTokenExpirationTime = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:ExpiryInDays"] ?? "7"));

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpirationTime = newRefreshTokenExpirationTime;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }

            var returnData = new ReturnAccessTokenDTO()
            {
                AccessToken = newAccessToken,
                ExpiresIn = Convert.ToInt32(_config["Jwt:ExpiryInMinutes"] ?? "5"),
                RefreshToken = newRefreshToken,
                RefreshTokenExpirationTime = newRefreshTokenExpirationTime
            };

            return returnData;
        }

        public async Task Logout(string refreshToken)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null)
            {
                throw new UnauthorizedException("There is no user with this refresh token");
            }

            if (user.RefreshTokenExpirationTime <= DateTime.Now)
            {
                throw new UnauthorizedException("Refresh token is expired");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpirationTime = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }
        }
    }
}
