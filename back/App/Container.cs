using Back.Domain.Services;
using Back.Infra.Database;
using Back.Infra.API;
using Back.Infra.Dependence;

namespace Back.App;
public class Settings
{
    public string DbName { get; set; } = "";
    public string DbUser { get; set; } = "";
    public string DbPassword { get; set; } = "";
    public string DbPort { get; set; } = "";
    public string DbHost { get; set; } = "";
    public string SecretKey { get; set; } = "";
    public string CepAbertoToken { get; set; } = "";

    public Settings (){
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

public class Container
{
    public Settings? Settings = new Settings();
    public UserService UserService;
    public DistanceService DistanceService;
    public TokenService TokenService;

    public Container()
    {
        var TokenDependence = new TokenDependence(Settings.SecretKey);
        var AuthDependence = new AuthDependence();

        var CepAbertoApi = new CepAbertoApi(Settings.CepAbertoToken);

        var options = AppDbContext.CreateOptions(
            Settings.DbHost,
            Settings.DbName,
            Settings.DbUser,
            Settings.DbPassword,
            Settings.DbPort
        );

        var DbContext = new AppDbContext(options);
        var UserRepository = new UserRepository(DbContext);
        var DistanceRepository = new DistanceRepository(DbContext);

        UserService = new UserService(UserRepository, AuthDependence);
        DistanceService = new DistanceService(DistanceRepository, CepAbertoApi);
        TokenService = new TokenService(UserRepository, TokenDependence);
    }
}