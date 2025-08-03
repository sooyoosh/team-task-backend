namespace TeamTaskManager.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<TaskItemDto> Tasks { get; set; }
    }
}
