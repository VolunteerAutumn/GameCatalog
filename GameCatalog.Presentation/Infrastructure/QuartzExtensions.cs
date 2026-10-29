using Quartz;
using GameCatalog.BLL.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace GameCatalog.Presentation.Infrastructure;

public static class QuartzExtensions
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("BirthdayEmailJob");
            q.AddJob<BirthdayEmailJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("BirthdayEmailJob-trigger")
                .WithCronSchedule("0 0 9 * * ?") // Runs at 9:00 AM every day
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }
}