using Microsoft.Extensions.DependencyInjection;
using Back.Infra.Database;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("PostgresConnection");

builder.Services.AddDatabaseConfiguration(connectionString);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
