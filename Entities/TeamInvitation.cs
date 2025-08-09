namespace TeamTaskManager.Entities
{
    public class TeamInvitation
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int InvitedUserId { get; set; }
        public User InvitedUser { get; set; }

        public int InvitedByUserId { get; set; }  // معمولاً مالک تیم یا کسی که دعوت کرده
        public User InvitedByUser { get; set; }

        public DateTime InvitedAt { get; set; } = DateTime.UtcNow;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    }
    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
