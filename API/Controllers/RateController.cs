using API.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/rate")]
    [Authorize]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public RateController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RateUser(int targetUserId, int rating)
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is null) return BadRequest("No such user");

            await _userRepository.SetUserRating(userId.Value, targetUserId, rating);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRatingFromUser(int targetUserId)
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            if (userId is null) return BadRequest("No such user");

            var rating = await _userRepository.GetUserRating(userId.Value, targetUserId);
            
            if (rating is null) return BadRequest("No rating");

            return Ok(rating);
        }

        [HttpGet("fromall")]
        public async Task<IActionResult> GetRatingFromAllUsers(int targetUserId)
        {
            var rating = await _userRepository.GetUserRatingFromAll(targetUserId);

            if (rating is null) return BadRequest("No rating");

            return Ok(rating);
        }
    }
}
