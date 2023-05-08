namespace ChatCodeChallenge.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; } 
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
