
using Back.Infra.Database;
using Serilog;

namespace Back.Infra.Builder;
public class Build
{
    public static WebApplicationBuilder CreateBuilder(string[] args)
    {
        string dbString = AppDbContext.genetateStringbySettings(Program.Settings);

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDatabaseConfiguration(dbString);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Month)
            .CreateLogger();

        builder.Logging.AddSerilog();


        builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:7001", "http://localhost:7000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

        return builder;
    }
}