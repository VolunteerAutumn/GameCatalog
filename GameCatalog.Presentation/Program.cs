global using Microsoft.Extensions.DependencyInjection;
using GameCatalog.BLL;
using GameCatalog.DAL;
using GameCatalog.DAL.Data;
using GameCatalog.Presentation.Infrastructure;
using GameCatalog.Presentation.Middleware;
using Microsoft.AspNetCore.Identity;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Logging
Log.Logger = new LoggerConfiguration().WriteTo.File("logs/app.log").CreateLogger();
builder.Host.UseSerilog();

// Core Services
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

// Custom Layer DI
builder.Services.AddBll(builder.Configuration);

// Infrastructure
builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddBackgroundJobs();

var app = builder.Build();

// Database and role seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GameCatalogDbContext>();
    await Seeder.SeedAsync(context);

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    async Task EnsureRoleAsync(string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }
    await EnsureRoleAsync("User");
    await EnsureRoleAsync("Admin");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Pipeline
app.UseMiddleware<ExceptionHandleMW>();
app.UseMiddleware<TimestampMW>();
app.UseMiddleware<BasicInfoMW>();
app.UseMiddleware<IPMW>();
app.UseMiddleware<RequestCounterMW>();
app.UseMiddleware<ApiKeyMW>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();