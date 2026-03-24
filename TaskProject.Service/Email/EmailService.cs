using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Services.Contract;

namespace TaskProject.Service.Email
{
    public class EmailService:IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    "test123@gmail.com",
                    "APP_PASSWORD"
                ),
                EnableSsl = true
            };

            var mail = new MailMessage(
                "test123@gmail.com",
                to,
                subject,
                body
            );

            await client.SendMailAsync(mail);
        }
    }
}
