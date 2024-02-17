using System.Net;
using System.Net.Mail;
using AspNetEmailExample.Models;
using Assignment.Models;

    namespace Assignment.Services
    {
        public class EmailService
        {
            private readonly ApplicationDbContext _dbContext;

            public EmailService(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task SendEmailToMultipleRecipientsAsync(string fromEmail, string subject, string body, IEnumerable<string> recipientEmails)
            {
                // Check if the daily email limit has been reached for the sender's email address
                if (IsDailyEmailLimitReached(fromEmail))
                {
                    throw new InvalidOperationException("The daily email limit has been reached for the sender's email address.");
                }

                // Create a list to store the status of each email sent
                var emails = new List<SentEmail>();

                // Send an email to each recipient
                foreach (var recipientEmail in recipientEmails)
                {
                    // Create a new email message
                    var mailMessage = new MailMessage(fromEmail, recipientEmail)
                    {
                        Subject = subject,
                        Body = body
                    };

                    // Send the email using the SMTP client
                    using (var client = new SmtpClient("smtp.example.com"))
                    {
                        client.Credentials = new NetworkCredential(fromEmail, "password");
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                    }

                    // Add the status of the email to the list
                    emails.Add(new SentEmail
                    {
                        SentTo = recipientEmail,
                        SentDate = DateTime.UtcNow,
                        Status = "Sent"
                    });
                }

                // Save the status of each email to the database
                foreach (var email in emails)
                {
                    _dbContext.SentEmails.Add(email);
                }
                await _dbContext.SaveChangesAsync();
            }

            private bool IsDailyEmailLimitReached(string fromEmail)
            {
                // Calculate the number of emails sent from the sender's email address today
                var emailsSentToday = _dbContext.SentEmails
                    .Where(e => e.SentDate.Date == DateTime.UtcNow.Date && e.SentTo == fromEmail)
                    .Count();

                // Check if the daily email limit has been reached
                if (emailsSentToday >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
