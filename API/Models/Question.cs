using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Question
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("question_number")]
        public int QuestionNumber { get; set; }

        [Column("question_text")]
        public string QuestionText { get; set; } = string.Empty;

        [Column("matching_info_column_name")]
        public string MatchingInfoColumnName { get; set; } = string.Empty;
    }
}
