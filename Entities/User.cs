using System.ComponentModel.DataAnnotations;

namespace TeamTaskManager.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];

        public string? ProfileImageUrl { get; set; }
        public ICollection<Team> OwnedTeams { get; set; } = new List<Team>();

        public ICollection<TeamMember> Teams { get; set; } = new List<TeamMember>();
    }
}
