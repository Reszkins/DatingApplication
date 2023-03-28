using System.ComponentModel.DataAnnotations.Schema;

namespace API.Dtos
{
    public class UserInfoDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
