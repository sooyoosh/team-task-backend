namespace TeamTaskManager.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITeamRepository TeamRepository { get; }
         

        Task<bool> CompleteAsync();
        bool HasChanges();
    }
}
