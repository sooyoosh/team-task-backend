using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using TeamTaskManager.DTOs;
using TeamTaskManager.Entities;
using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Controller
{

    [ApiController]
    [Route("api")]

    public class AuthController:ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        
        public AuthController(IUnitOfWork uow, ITokenService tokenService,IMapper mapper)
        {
            _unitOfWork = uow;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody]RegisterDto dto)
        {
            if (await _unitOfWork.UserRepository.UserExists(dto.Email))
                return BadRequest("Email is already registered");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            _unitOfWork.UserRepository.Add(user);
            var result = await _unitOfWork.CompleteAsync();

            if (!result) return BadRequest("Problem registering user");

            // لود اطلاعات کامل‌تر بعد از ثبت
            var userWithTeams = await _unitOfWork.UserRepository.GetByEmailWithTeamsAsync(user.Email);
            var userDto = _mapper.Map<UserDto>(userWithTeams);
            userDto.Token = await _tokenService.CreateToken(user);

            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailWithTeamsAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid email or password");
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = await _tokenService.CreateToken(user);

            return Ok(userDto);
        }
    }
}
