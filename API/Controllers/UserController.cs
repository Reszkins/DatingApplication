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
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public UserController(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        [HttpGet, Route("id")]
        public async Task<IActionResult> GetUserId(string email)
        {
            var user = await _accountRepository.GetUser(email);

            if(user is not null)
            {
                return Ok(user.Id);
            }
            else
            {
                return BadRequest("No such email in database");
            }
        }

        [HttpGet, Route("description")]
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
