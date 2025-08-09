using TeamTaskManager.Entities;

namespace TeamTaskManager.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public required string Token { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }

        public List<TeamDto>? OwnedTeams { get; set; } = [];
        public List<TeamDto>? Teams { get; set; } = [];

    }
}
