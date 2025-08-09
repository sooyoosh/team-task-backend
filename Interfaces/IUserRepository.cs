using TeamTaskManager.Entities;

namespace TeamTaskManager.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailWithTeamsAsync(string email);
        Task<bool> UserExists(string email);
        void Add(User user);
        Task UpdateProfileImageAsync(int userId, string imageUrl);
        Task<List<User>> GetAllUsers();
    }
}
