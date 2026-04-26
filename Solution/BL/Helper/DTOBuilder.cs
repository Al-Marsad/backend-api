using AutoMapper;
using BL.DTO.User;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BL.Helper
{
    public class DTOBuilder
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public DTOBuilder(IMapper mapper, UserManager<AppUser> userManager)
        {
            this._mapper = mapper;
            this._userManager = userManager;
        }
        public async Task<GetUserPorfileDTO> BuildUserProfileDTO(AppUser user)
        {
            var dto = _mapper.Map<GetUserPorfileDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            dto.Role = string.Join(",", roles);
            return dto;
        }
    }
}
