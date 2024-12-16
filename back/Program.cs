using Back.Infra.Database;
using Back.App;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.Configure<AppDbContext>(builder.Configuration.GetSection("AppDbContext"));

DotNetEnv.Env.Load(".env");
string connectionString =
    $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

builder.Services.AddDatabaseConfiguration(connectionString);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
