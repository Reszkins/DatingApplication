namespace API.Dtos
{
    public class MessageDto
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTimeOffset DateTime { get; set; }
    }
}
