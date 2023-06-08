using API.DataAccess.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Route("api/photo")]
    [Authorize]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private readonly PhotoService _photoService;
        private readonly IUserRepository _userRepository;

        public PhotoController(PhotoService photoService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _photoService = photoService;
        }


        [HttpGet, Route("all")]
        public async Task<IActionResult> GetPhotos()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));
            if (userId is null)
            {
                return BadRequest("User does not exist.");
            }

            return await GetPhotosByUserId(userId.Value.ToString());
        }

        [HttpGet, Route("all/{userId}")]
        public async Task<IActionResult> GetPhotos(int userId)
        {
            return await GetPhotosByUserId(userId.ToString());
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));
            if (userId is null)
            {
                return BadRequest("User does not exist.");
            }
            if (Request.Form.Files.Count > 0)
            {
                var photoFile = Request.Form.Files[0];
                using (var photoStream = photoFile.OpenReadStream())
                {
                  
                    await _photoService.SavePhoto(photoStream, userId.Value);
                }

                return Ok("Photo uploaded successfully.");
            }

            return BadRequest("No photo file found in the request.");
        }

        private async Task<IActionResult> GetPhotosByUserId(String userId)
        {
            string userFolderPath = Path.Combine("/app/Photos/", userId);

            if (!Directory.Exists(userFolderPath))
            {
                return NotFound("User folder does not exist.");
            }

            string[] photoFiles = Directory.GetFiles(userFolderPath, "*.txt");
            if (photoFiles.Length == 0)
            {
                return NotFound("No photo files found.");
            }

            string firstPhotoFile = photoFiles[0];
            string fileName = Path.GetFileNameWithoutExtension(firstPhotoFile);
            string base64Image = await System.IO.File.ReadAllTextAsync(firstPhotoFile);

            if (base64Image.StartsWith("data:image/jpeg;base64,"))
            {
                base64Image = base64Image.Substring("data:image/jpeg;base64,".Length);
            }

            var photoJson = new { photo = base64Image };

            return Ok(photoJson);
        }
    }
}
