using Microsoft.EntityFrameworkCore;

namespace Back.Infra.Database;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
