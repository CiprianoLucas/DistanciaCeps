using Back.Infra.Builder;
namespace Back;

public class Settings
{
    public string DbName { get; set; } = "";
    public string DbUser { get; set; } = "";
    public string DbPassword { get; set; } = "";
    public string DbPort { get; set; } = "";
    public string DbHost { get; set; } = "";
    public string SecretKey { get; set; } = "";
    public string CepAbertoToken { get; set; } = "";

    public Settings()
    {
        DotNetEnv.Env.Load(".env");
        DbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "default_db";
        DbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "user";
        DbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
        DbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        DbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "secret_key";
        CepAbertoToken = Environment.GetEnvironmentVariable("CEP_ABERTO_TOKEN") ?? "cep_aberto_token";
    }
}
public class Program
{
    public static Settings Settings = new Settings();
    public static void Main(string[] args)
    {

        var Build = new Build();
        var builder = Build.CreateBuilder(args);
        var app = builder.Build();
        app.UseCors("AllowAll");
        app.Urls.Add("http://*:7000");
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}