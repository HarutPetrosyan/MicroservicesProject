using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Email
{
    public class EmailService:IEmailService
    {
        public EmailSettings _EmailSetting { get; }

        public ILogger<EmailService> _Logger { get; }

        public EmailService(IOptions<EmailSettings> emailSetting, ILogger<EmailService> logger)
        {
            _EmailSetting = emailSetting.Value;
            _Logger = logger;
        }
        public async Task<bool> SendEmail(Application.Models.Email email)
        {
           var client=new SendGridClient(_EmailSetting.ApiKey);
           var subject = email.Subject;
           var to = new EmailAddress(email.To);
           var emailBody = email.Body;

           var from = new EmailAddress
           {
               Email = _EmailSetting.FromAddress,
               Name = _EmailSetting.FromName
           };

           var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
           var response = await client.SendEmailAsync(sendGridMessage);

           _Logger.LogInformation("Email sent.");

           if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
               return true;

           _Logger.LogError("Email sending failed.");
           return false;
        }
    }
}
