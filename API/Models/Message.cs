using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Message
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("from_user")]
        public int FromUser { get; set; }

        [Column("to_user")]
        public int ToUser { get; set; }

        [Column("message_text")]
        public string MessageText { get; set; } = string.Empty;

        [Column("sent_timestamp")]
        public long SentTimestamp { get; set; }
    }
}
