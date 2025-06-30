using System.ComponentModel.DataAnnotations;

namespace TeamTaskManager.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
