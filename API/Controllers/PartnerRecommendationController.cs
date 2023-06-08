using API.DataAccess.Repositories;
using API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/partnerrecommendation")]
    [Authorize]
    [ApiController]
    public class PartnerRecommendationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public PartnerRecommendationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendedPartners()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5000/matches?user_id={userId.Value}&num_matches=5");

            var partners = await response.Content.ReadFromJsonAsync<List<RecommendedPartnerReponseDto>>();

            if (partners is null)
            {
                return Ok("No recommendations");
            }

            var partnersToSend = new List<RecommendedPartnerDto>();
            foreach (var partner in partners)
            {
                partnersToSend.Add(new RecommendedPartnerDto
                {
                    RecommendedUserId = partner.TargetUserId,
                    RecommendationScore = partner.Score
                });
            }

            return Ok(partnersToSend);
        }
    }
}
