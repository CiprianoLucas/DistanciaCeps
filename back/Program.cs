using Back.Infra.Database;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load(".env");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                     .AddEnvironmentVariables();

var configuration = builder.Configuration;
var settingsSection = configuration.GetSection("Settings");

var connectionString = "Host=${DB_HOST};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD}"
    .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "default_db")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "user")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password");

builder.Services.AddDatabaseConfiguration(connectionString);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Month)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
