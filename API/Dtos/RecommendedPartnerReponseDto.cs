using System.Text.Json.Serialization;

namespace API.Dtos
{
    public class RecommendedPartnerReponseDto
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("target_user_id")]
        public int TargetUserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }
}
