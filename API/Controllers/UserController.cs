using API.DataAccess.Repositories;
using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet, Route("id")]
        public async Task<IActionResult> GetUserId()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is not null)
            {
                return Ok(userId.Value);
            }
            else
            {
                return BadRequest("No such email in database");
            }
        }

        [HttpGet, Route("id/{email}")]
        public async Task<IActionResult> GetUserId(string email)
        {
            var userId = await _userRepository.GetUserId(email);

            if(userId is not null)
            {
                return Ok(userId);
            }
            else
            {
                return BadRequest("No such email in database");
            }
        }

        [HttpGet, Route("email")]
        public async Task<IActionResult> GetUserEmail(int id)
        {
            var userEmail = await _userRepository.GetUserEmail(id);

            if (userEmail is not null)
            {
                return Ok(userEmail);
            }
            else
            {
                return BadRequest("No such id in database");
            }
        }

        [HttpGet, Route("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));
            var user = await _userRepository.GetUserBaseInfo(userId.Value);

            if (user is not null)
            {
                return Ok(new UserInfoDto
                {
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth
                });
            }
            else
            {
                return BadRequest("No such email in database");
            }
        }

        [HttpGet, Route("info/{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var user = await _userRepository.GetUserBaseInfo(userId);

            if (user is not null)
            {
                return Ok(new UserInfoDto
                {
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth
                });
            }
            else
            {
                return BadRequest("No such email in database");
            }
        }

        [HttpGet, Route("description")]
        public async Task<IActionResult> GetUserDescription()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));
            var description = await _userRepository.GetUserDescription(userId.Value);

            if (description is not null)
            {
                return Ok(description);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpGet, Route("description/{userId}")]
        public async Task<IActionResult> GetUserDescription(int userId)
        {
            var description = await _userRepository.GetUserDescription(userId);

            if (description is not null)
            {
                return Ok(description);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpPost, Route("description")]
        public async Task<IActionResult> SetUserDescription([FromBody] DescriptionDto description)
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is not null)
            {
                await _userRepository.SetUserDescription(userId.Value, description.Description);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpDelete, Route("description")]
        public async Task<IActionResult> DeleteUserDescription()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is not null)
            {
                await _userRepository.DeleteUserDescription(userId.Value);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
