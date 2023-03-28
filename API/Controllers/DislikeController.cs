using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/dislike")]
    [Authorize]
    [ApiController]
    public class DislikeController
    {
        [HttpGet, Route("all")]
        public async Task<IActionResult> GetUserDislikes()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserDislike()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserDislike()
        {
            throw new NotImplementedException();
        }
    }
}
