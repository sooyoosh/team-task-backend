using TeamTaskManager.Entities;

namespace TeamTaskManager.DTOs
{
    public class UserDto
    {
        public string FullName { get; set; }
        public required string Token { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }

        public List<TeamDto>? OwnedTeams { get; set; } = [];
        public List<TeamDto>? Teams { get; set; } = [];

    }
}
