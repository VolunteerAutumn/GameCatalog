using GameCatalog.BLL;
using GameCatalog.DAL;
using GameCatalog.DAL.Data;
using GameCatalog.Presentation.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/app.log")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "ApiKey",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "X-API-KEY",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter your API key"
        });

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddBll(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<GameCatalogDbContext>();

    await Seeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<TimestampMW>();

app.UseMiddleware<BasicInfoMW>();

app.UseMiddleware<IPMW>();

app.UseMiddleware<RequestCounterMW>();

app.UseMiddleware<ApiKeyMW>();

app.MapControllers();

app.Run();