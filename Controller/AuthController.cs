using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

            // Generate Tokens
            var accessToken = await _tokenService.CreateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Add refresh token to user and save
            user.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CompleteAsync(); // 


            // لود اطلاعات کامل‌تر بعد از ثبت
            var userWithTeams = await _unitOfWork.UserRepository.GetByEmailWithTeamsAsync(user.Email);
            var userDto = _mapper.Map<UserDto>(userWithTeams);
            userDto.Token= accessToken;
            userDto.RefreshToken=refreshToken.Token;
            

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

            

            // Generate Tokens
            var accessToken = await _tokenService.CreateToken(user);
            var refreshToken =  _tokenService.GenerateRefreshToken();

            // Add refresh token to user and save
            user.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CompleteAsync(); // 

            // Return DTO
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = accessToken;
            userDto.RefreshToken = refreshToken.Token;

          

            return Ok(userDto);
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDto>> GetRefreshToken([FromBody] RefreshTokenRequest request)
        {


            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
                return BadRequest("Invalid access token");

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Invalid token");

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(int.Parse(userId));
            if (user == null) 
                return Unauthorized("user not found") ;
            var storedRefreshToken = user.RefreshTokens.FirstOrDefault(rt=>rt.Token==request.RefreshToken); //
            if (storedRefreshToken == null ||
                storedRefreshToken.Expires < DateTime.UtcNow ||
                storedRefreshToken.IsUsed ||
                storedRefreshToken.IsRevoked)
            {
                return Unauthorized("Invalid refresh token");
            }



            storedRefreshToken.IsUsed = true;
            storedRefreshToken.IsRevoked=true;
            await _unitOfWork.CompleteAsync();

            var accessToken = await _tokenService.CreateToken(user);
            var refreshNewToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshNewToken);

            await _unitOfWork.CompleteAsync();

            // Return DTO
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = accessToken;
            userDto.RefreshToken = refreshNewToken.Token;



            return Ok(userDto);




        }






    }
}
