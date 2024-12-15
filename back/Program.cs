using Back.Infra.Database;

var builder = WebApplication.CreateBuilder(args);

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
