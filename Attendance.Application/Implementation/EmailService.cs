using Attendance.Application.Abstractions.Services;
using Attendance.Domain.Common;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Attendance.Application.Implementation;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSettings _emailSettings;
    
    public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options)
    {
        _logger = logger;
        _emailSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }
    
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
        _logger.LogInformation("Email body: {Body}", body);
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("NoReply", _emailSettings.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        
        try 
        {
            var builder = new BodyBuilder
            {
                TextBody = body,
                HtmlBody = $"<p>{body}</p>"
            };
            message.Body = builder.ToMessageBody();
            
            using var smtp = new SmtpClient();
            var useSsl = _emailSettings.UseSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, useSsl, ct);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.SmtpServer, ct);
            await smtp.SendAsync(message, ct);
            await smtp.DisconnectAsync(true, ct);
            
            _logger.LogInformation("Email sent to {Recipient} with subject {Subject}", to, subject);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw;
        }
    }
}