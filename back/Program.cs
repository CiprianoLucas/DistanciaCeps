using Back.Infra.Builder;
using Back.Infra.Queue;
namespace Back;

public class Settings
{
    public string DbName { get; set; }
    public string DbUser { get; set; }
    public string DbPassword { get; set; }
    public string DbPort { get; set; }
    public string DbHost { get; set; }
    public string SecretKey { get; set; }
    public string CepAbertoToken { get; set; }
    public string Host { get; set; }
    public string RMqHostName { get; set; }
    public string RMqUserName { get; set; }
    public string RMqPassword { get; set; }

    public Settings()
    {
        DotNetEnv.Env.Load(".env");
        DbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "distancia-ceps";
        DbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
        DbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "admin";
        DbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        DbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "MinhaSuperSecretaChaveComMaisDe32Caracteres123456";
        CepAbertoToken = Environment.GetEnvironmentVariable("CEP_ABERTO_TOKEN") ?? "cep_aberto_token";
        Host = Environment.GetEnvironmentVariable("HOST") ?? "localhost";
        RMqHostName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "admin";
        RMqUserName = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "admin";
        RMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_VHOST") ?? "localhost";
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

        Thread apiThread = new Thread(app.Run);
        apiThread.Start();

        Thread rabbitMqThread = new Thread(QueueConsumer);
        rabbitMqThread.Start();

        Console.ReadLine();
    }
    public static void QueueConsumer()
    {
        // Queue.ConsumerLoop(Settings);
    }

}