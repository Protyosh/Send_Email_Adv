namespace Assignment.Models
{
    public class Recipient
    {
        public int RecipientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<SentEmail> SentEmails { get; set; }
    }
}
