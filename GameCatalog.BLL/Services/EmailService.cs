using GameCatalog.BLL.Interfaces;
using GameCatalog.BLL.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GameCatalog.BLL.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        // Always print to console first (so you have the link if the real email fails)
        Console.WriteLine("\n==================================================");
        Console.WriteLine($"📧 ATTEMPTING TO SEND EMAIL TO: {toEmail}");
        Console.WriteLine($"SUBJECT: {subject}");
        Console.WriteLine($"BODY:\n{htmlMessage}");
        Console.WriteLine("==================================================\n");

        if (_settings.UseFakeSender)
        {
            Console.WriteLine("⚠️ Fake sender enabled. Skipping real SMTP send.");
            return;
        }

        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            Console.WriteLine("✅ REAL EMAIL SENT SUCCESSFULLY!");
        }
        catch (Exception ex)
        {
            // If the real email fails, we catch the error so the API doesn't crash.
            // The link is already printed at the top of this console output!
            Console.WriteLine($"❌ SMTP FAILED: {ex.Message}");
            Console.WriteLine("⚠️ Don't worry, you can use the link printed above.");
        }
    }
}