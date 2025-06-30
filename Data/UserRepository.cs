using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Entities;
using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Data
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.OwnedTeams)
                .Include(u => u.Teams)
                    .ThenInclude(tm => tm.Team)
                .FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetByEmailWithTeamsAsync(string email)
        {
            return await _context.Users
                .Include(u => u.OwnedTeams)
                .Include(u => u.Teams)
                    .ThenInclude(tm => tm.Team)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public async Task UpdateProfileImageAsync(int userId, string imageUrl)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");
            user.ProfileImageUrl = imageUrl;
      
        }
    }
}
