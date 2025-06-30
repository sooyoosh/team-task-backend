using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Entities;
using TeamTaskManager.Extension;
using TeamTaskManager.Interfaces;

namespace TeamTaskManager.Controller
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IMapper mapper,IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost("upload-profile-image")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            var userId = User.GetUserId();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }



            var user=await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            
            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                var oldFileName = Path.GetFileName(user.ProfileImageUrl);
                var oldImagePath = Path.Combine(folderPath, oldFileName);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }


            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folderPath,fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var imageUrl = $"{baseUrl}/images/{fileName}";
            await _unitOfWork.UserRepository.UpdateProfileImageAsync(userId, imageUrl);
            await _unitOfWork.CompleteAsync();

            return Ok(new { imageUrl });
        }


    }
}
