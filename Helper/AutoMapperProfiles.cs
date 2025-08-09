using AutoMapper;
using TeamTaskManager.DTOs;
using TeamTaskManager.Entities;

namespace TeamTaskManager.Helper
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles() {


         CreateMap<Team, TeamDto>()
        .ForMember(dest => dest.OwnerFullName, opt => opt.MapFrom(src => src.Owner.FullName));

         CreateMap<User, UserDto>()
        .ForMember(dest => dest.Token, opt => opt.Ignore())
        .ForMember(dest => dest.OwnedTeams, opt => opt.MapFrom(src => src.OwnedTeams))
        .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.Teams.Select(tm => tm.Team)));


         CreateMap<Team, TeamDetailsDto>()
        .ForMember(dest => dest.OwnerFullName, opt => opt.MapFrom(src => src.Owner.FullName));


         CreateMap<TeamMember, MemberDto>()
         .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
         .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
         .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));


         CreateMap<Project, ProjectDto>();

         CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TeamInvitation, TeamInvitationDto>()
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.InvitedBy, opt => opt.MapFrom(src => src.InvitedByUser.FullName))
            .ForMember(dest => dest.InvitedAt, opt => opt.MapFrom(src => src.InvitedAt));



        }
    }
}
