using GameCatalog.DAL.Entities;
using GameCatalog.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Quartz;

namespace GameCatalog.BLL.Jobs;

public class BirthdayEmailJob : IJob
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<BirthdayEmailJob> _logger;

    public BirthdayEmailJob(
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ILogger<BirthdayEmailJob> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting Birthday Email Job...");

        var users = _userManager.Users
            .Where(u => u.IsSubscribedToNewsletter && u.DateOfBirth != null)
            .ToList();

        var today = DateTime.UtcNow.Date;

        foreach (var user in users)
        {
            var dob = user.DateOfBirth!.Value;

            // Check if today is their birthday
            if (dob.Month == today.Month && dob.Day == today.Day)
            {
                var emailBody = $"<h1>Happy Birthday, {user.UserName}!</h1><p>We hope you have an amazing day. 🎉</p>";
                await _emailService.SendEmailAsync(user.Email!, "Happy Birthday from GameCatalog!", emailBody);
                _logger.LogInformation($"Sent birthday email to {user.Email}");
            }
        }
    }
}