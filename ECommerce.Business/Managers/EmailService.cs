using ECommerce.Business.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace ECommerce.Business.Managers
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _cfg;
        public EmailService(IConfiguration cfg) => _cfg = cfg;

        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        {
            var host = _cfg["Email:SmtpServer"] ?? "srvm17.trwww.com";
            var user = _cfg["Email:Username"];               // örn: info@cypwood.com
            var pass = _cfg["Email:Password"];
            var fromName = _cfg["Email:FromName"] ?? "Cypwood";
            var timeout = int.TryParse(_cfg["Email:TimeoutMs"], out var t) ? t : 15000;

            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(fromName, user));    // From = login (çoğu sunucu ister)
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var smtp = new SmtpClient { Timeout = timeout };

            // 1) 465 / SSL
            try
            {
                await smtp.ConnectAsync(host, 465, SecureSocketOptions.SslOnConnect, ct);
            }
            catch
            {
                // 2) Olmazsa 587 / STARTTLS
                if (smtp.IsConnected) await smtp.DisconnectAsync(true, ct);
                await smtp.ConnectAsync(host, 587, SecureSocketOptions.StartTls, ct);
            }

            await smtp.AuthenticateAsync(user, pass, ct);
            await smtp.SendAsync(msg, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }

}
