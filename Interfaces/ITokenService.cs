using System.Security.Claims;
using TeamTaskManager.Entities;

namespace TeamTaskManager.Interfaces
{
    public interface ITokenService
    {

        Task<string> CreateToken(User user);
        RefreshToken GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);


    }
}
