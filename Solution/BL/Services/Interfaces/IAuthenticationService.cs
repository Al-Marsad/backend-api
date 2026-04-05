using BL.DTO.User;

namespace BL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<ReturnRegisteredUserDTO> Regsiter(AddUserDTO userDTO, string RoleName);
        //public Task<ReturnAuthDTO> Login(LoginUserDTO obj);
        //public Task<bool> Logout(int userId);
        //public Task<ReturnAuthDTO> Refresh(string refreshToken);
    }
}
