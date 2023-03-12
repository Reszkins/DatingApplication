using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class UserDescription
    {
        [Column("user_id")]
        public int Id { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;
    }
}
