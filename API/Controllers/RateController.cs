using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/rate")]
    [Authorize]
    [ApiController]
    public class RateController
    {

        [HttpPost]
        public async Task<IActionResult> RateUser(int userId, int rating)
        {
            throw new NotImplementedException();
        }
    }
}
