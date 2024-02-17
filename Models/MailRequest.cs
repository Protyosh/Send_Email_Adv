namespace Assignment.Models
{
    public class MailRequest
    {
        public string FromEmail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> ToEmails { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
