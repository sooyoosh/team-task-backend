using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Data;
using TeamTaskManager.DTOs;
using TeamTaskManager.Entities;
using TeamTaskManager.Extension;
using TeamTaskManager.Interfaces;
using TeamTaskManager.Services;

namespace TeamTaskManager.Controller
{
    [ApiController]
    [Route("api/team")]
    public class TeamController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TeamService _teamService;
        public TeamController(IMapper mapper, IUnitOfWork unitOfWork, TeamService teamService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _teamService = teamService;
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



        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDto dto)
        {
            var userId = User.GetUserId(); // استفاده از ClaimsPrincipalExtensions

            var existingTeam = await _unitOfWork.TeamRepository.GetTeamByIdAsync(id);
            if (existingTeam == null)
                return NotFound("Team not found");

            if (existingTeam.OwnerId != userId)
                return Forbid("You are not the owner of this team");

            // 
            existingTeam.Name = dto.Name;
            existingTeam.Description = dto.Description;

            _unitOfWork.TeamRepository.UpdateTeam(existingTeam);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<TeamDto>(existingTeam));
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var userId = User.GetUserId();
            var team = await _unitOfWork.TeamRepository.GetTeamByIdAsync(id);

            if (team == null)
                return NotFound("Team not found");

            if (team.OwnerId != userId)
                return Forbid("Only the owner can delete this team");

            _unitOfWork.TeamRepository.DeleteTeam(team);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTeamDetails(int id)
        {
            var team = await _unitOfWork.TeamRepository.GetTeamByIdAsync(id);

            if (team == null)
                return NotFound("Team not found");

            var teamDto = _mapper.Map<TeamDetailsDto>(team);
            return Ok(teamDto);
        }
        [HttpGet("inviteUser/{teamId}/{invitedUserId}")]
        [Authorize]
        public async Task<IActionResult> InviteUser([FromRoute] int teamId,int invitedUserId)
        {
            var currentUserId=User.GetUserId();
            var result = await _teamService.InviteUserToTeamAsync(teamId, invitedUserId, currentUserId);
            if (result) return Ok(result);
            else return BadRequest("خطا");

        }
        [HttpGet("invitations/pending")]
        [Authorize]
        public async Task<IActionResult> GetPendingInvitationsForUser()
        {
            var userId=User.GetUserId();
            var invites=await _unitOfWork.TeamRepository.GetPendingInvitationsForUserAsync(userId);
            var invitesDto = _mapper.Map<List<TeamInvitationDto>>(invites);
            return Ok(invitesDto);
        }

    }
}
