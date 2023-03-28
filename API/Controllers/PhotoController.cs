using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/photo")]
    [Authorize]
    [ApiController]
    public class PhotoController
    {
        [HttpGet, Route("all")]
        public async Task<IActionResult> GetPhotos()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePhoto()
        {
            throw new NotImplementedException();
        }
    }
}
