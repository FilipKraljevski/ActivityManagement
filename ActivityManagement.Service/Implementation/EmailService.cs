using ActivityManagement.Domain;
using ActivityManagement.Service.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Service.Implementation
{
    public class EmailService : IEmailService
    {
        public readonly EmailSettings emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void Send(string toEmail, string dateFrom, string dateTo, string userId)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailSettings.EmailDisplayName, emailSettings.SmtpUserName));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = "Report Sent";
            string url = "https://localhost:44361/Email/Save?userId=" + userId + "&from=" + dateFrom + "&to=" + dateTo;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = url };

            using var smtp = new SmtpClient();
            var socketOptions = emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            smtp.Connect(emailSettings.SmtpServer, emailSettings.SmtpServerPort, socketOptions);
            smtp.Authenticate(emailSettings.SmtpUserName, emailSettings.SmtpPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
