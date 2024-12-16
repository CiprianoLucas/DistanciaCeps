using Back.Domain.Services;
using Back.Domain.Entities;
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