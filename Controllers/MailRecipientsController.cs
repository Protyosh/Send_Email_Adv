
using AspNetEmailExample.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetEmailExample.Controllers
{
    public class MailRecipientsController : Controller
    {
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _dbContext;

        public MailRecipientsController(EmailService emailService, ApplicationDbContext dbContext)
        {
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            // Get the list of recipients from the database
            var recipients = await _dbContext.Recipients.Include(r => r.SentEmails).ToListAsync();

            return View(recipients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(string fromEmail, string subject, string body, string[] recipientEmails)
        {
            try
            {
                // Send an email to each recipient
                foreach (var recipientEmail in recipientEmails)
                {
                    await _emailService.SendEmailToMultipleRecipientsAsync(fromEmail, subject, body, new[] { recipientEmail });
                }

                // Redirect to the index action with a success message
                TempData["Message"] = "The email has been sent to the selected recipients.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                // Display an error message if the daily email limit has been reached
                ViewBag.ErrorMessage = ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> EmailStatus()
        {
            // Get the list of sent emails from the database
            var emails = await _dbContext.SentEmails.OrderByDescending(e => e.SentDate).ToListAsync();

            return View(emails);
        }
    }
}
