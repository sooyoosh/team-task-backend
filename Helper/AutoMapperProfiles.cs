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

        
        }
    }
}
