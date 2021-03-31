using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Application.Common.Exceptions;
using Travel.Application.Common.Interfaces;
using Travel.Application.Dtos.Email;
using Travel.Domain.Settings;

namespace Travel.Shared.Services
{
    public class EmailService : IEmailService
    {

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            MailSettings = mailSettings.Value;
            _logger = logger;
        }

        public MailSettings MailSettings { get; }

        private ILogger<EmailService> _logger;

        public async Task SendAsync(EmailDto request)
        {
            try
            {
                var email = new MimeMessage { Sender = MailboxAddress.Parse(request.From ?? MailSettings.EmailFrom) };
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                var builder = new BodyBuilder { HtmlBody = request.Body };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }
    }
}
