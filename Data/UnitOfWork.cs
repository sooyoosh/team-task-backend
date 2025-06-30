using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        public UnitOfWork(AppDbContext context, IUserRepository userRepository, ITeamRepository teamRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public ITeamRepository TeamRepository => _teamRepository;

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
