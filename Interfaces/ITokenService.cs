using TeamTaskManager.Entities;

namespace TeamTaskManager.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
