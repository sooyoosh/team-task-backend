using System.ComponentModel.DataAnnotations;

namespace TeamTaskManager.Entities
{
    public class Team
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }


        // مالک تیم
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        // Navigation
        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
