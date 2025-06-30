using TeamTaskManager.Entities;

namespace TeamTaskManager.Interfaces
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task AddTeamAsync(Team team);
        void UpdateTeam(Team team);
        void DeleteTeam(Team team);
        Task<bool> TeamExistsAsync(int id);
    }
}
