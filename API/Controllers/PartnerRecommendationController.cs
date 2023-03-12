using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/partnerrecommendation")]
    [Authorize]
    [ApiController]
    public class PartnerRecommendationController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRecommendedPartners()
        {
            throw new NotImplementedException();
        }
    }
}
