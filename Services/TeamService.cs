using TeamTaskManager.Entities;
using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Services
{
    public class TeamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> InviteUserToTeamAsync(int teamId, int invitedUserId, int currentUserId)
        {
            var team = await _unitOfWork.TeamRepository.GetTeamByIdAsync(teamId);
            if (team == null)
                throw new Exception("تیم یاقت نشد");

            if (team.OwnerId != currentUserId)
                throw new Exception("فقط مالک تیم می‌تواند دعوت‌نامه ارسال کند");

            var existingInvitation = await _unitOfWork.TeamRepository
                .GetPendingInvitationAsync(teamId, invitedUserId);

            if (existingInvitation != null)
                throw new Exception("دعوت‌نامه قبلاً ارسال شده است");

            var invitation = new TeamInvitation
            {
                TeamId = teamId,
                InvitedUserId = invitedUserId,
                InvitedByUserId = currentUserId,
                InvitedAt = DateTime.UtcNow,
                Status = InvitationStatus.Pending
            };

            await _unitOfWork.TeamRepository.AddInvitationAsync(invitation);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
