using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Data;
using TeamTaskManager.DTOs;
using TeamTaskManager.Entities;
using TeamTaskManager.Extension;
using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Controller
{
    [ApiController]
    [Route("api/team")]
    public class TeamController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TeamController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDto dto)
        {
            var userId = User.GetUserId();
            var team = new Team
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = userId,
            };

            await _unitOfWork.TeamRepository.AddTeamAsync(team);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<TeamDto>(team));
        }


    }
}
