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

        public void Send(string toEmail, string link)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailSettings.EmailDisplayName, emailSettings.SmtpUserName));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = "Report Sent";
            string text = link + "\n" +
                    //"The code to access the link " + code + "\n" +
                    "The link will expire in 48 hours counting from the time the email was sent";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = text };

            using var smtp = new SmtpClient();
            var socketOptions = emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            smtp.Connect(emailSettings.SmtpServer, emailSettings.SmtpServerPort, socketOptions);
            smtp.Authenticate(emailSettings.SmtpUserName, emailSettings.SmtpPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
