using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Entities;
using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams
                .Include(t => t.Owner)
                .Include(t => t.Members)
                .ThenInclude(t => t.User)
                .Include(t => t.Projects)
                .ThenInclude(t => t.Tasks)
                .ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.Owner)
                .Include(t => t.Members)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public void UpdateTeam(Team team)
        {
            _context.Teams.Update(team);
        }

        public void DeleteTeam(Team team)
        {
            _context.Teams.Remove(team);
        }

        public async Task<bool> TeamExistsAsync(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }
    }
}
