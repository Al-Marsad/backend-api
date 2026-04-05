using DAL.Entities;

namespace BL.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(AppUser user, IList<string> roles);
        string GenerateRefreshToken();
    }
}
