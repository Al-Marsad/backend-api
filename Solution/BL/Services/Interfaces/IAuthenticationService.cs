using BL.DTO.User;

namespace BL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<ReturnRegisteredUserDTO> Regsiter(AddUserDTO userDTO, string RoleName);
        public Task<ReturnLoginUserDTO> Login(LoginUserDTO obj);
        public Task Logout(string refreshToken);
        public Task<ReturnAccessTokenDTO> Refresh(string refreshToken);
    }
}
