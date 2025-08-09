namespace TeamTaskManager.DTOs
{
    public class TeamInvitationDto
    {
        public int InvitationId { get; set; }
        public string TeamName { get; set; }
        public string InvitedBy { get; set; }
        public DateTime InvitedAt { get; set; }
    }
}
