using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class UserBaseInfo
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Column("second_name")]
        public string SecondName { get; set; } = string.Empty;

        [Column("gender")]
        public string Gender { get; set; } = string.Empty;

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
