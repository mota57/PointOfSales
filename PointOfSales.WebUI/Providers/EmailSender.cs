using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PointOfSales.WebUI.Providers
{
    //https://dotnetcoretutorials.com/2017/08/20/sending-email-net-core-2-0/
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            logger.LogDebug($"<EMAIL-SEND>\nemail::{email}\nSubject::{subject}\n<EMAIL-SEND>");
            return Task.CompletedTask;
        }
    }
}
