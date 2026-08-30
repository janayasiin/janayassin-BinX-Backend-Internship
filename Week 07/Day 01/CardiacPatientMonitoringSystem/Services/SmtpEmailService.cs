using System.Net;
using System.Net.Mail;

namespace CardiacPatientMonitoringSystem.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
    {
        var smtpHost = _configuration["EmailSettings:SmtpServer"];

        var smtpPort = int.Parse(
            _configuration["EmailSettings:Port"]!
        );

        var smtpEmail =
            _configuration["EmailSettings:SenderEmail"];

        var smtpPassword =
            _configuration["EmailSettings:SenderPassword"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(
                smtpEmail,
                smtpPassword)
        };

        using var message = new MailMessage
        {
            From = new MailAddress(smtpEmail!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}

