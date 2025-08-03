namespace TeamTaskManager.DTOs
{
    public class TeamDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerFullName { get; set; }
        public List<MemberDto> Members { get; set; }
        public List<ProjectDto> Projects { get; set; }
    }
}
