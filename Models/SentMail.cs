namespace Assignment.Models
{
    public class SentEmail
    {
        public int SentEmailId { get; set; }
        public DateTime SentDate { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SentTo { get; set; }
        public string Status { get; set; }

        public int RecipientId { get; set; }
        public virtual Recipient Recipient { get; set; }
    }
}
