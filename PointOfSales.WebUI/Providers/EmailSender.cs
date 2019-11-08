using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace PointOfSales.WebUI.Providers
{
    //https://dotnetcoretutorials.com/2017/08/20/sending-email-net-core-2-0/
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
