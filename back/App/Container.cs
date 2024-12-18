using Back.Domain.Services;
using Back.Infra.Database;
using Back.Infra.API;
using Back.Infra.Dependence;
using Back.Infra.Cash;

namespace Back.App;


public class Container
{
    public UserService UserService;
    public DistanceService DistanceService;
    public TokenService TokenService;

    public Container()
    {
        var settings = Program.Settings;
        var cash = Cash.Connect(settings);

        var TokenDependence = new TokenDependence(settings.SecretKey);
        var AuthDependence = new AuthDependence();
        var client = new HttpClient();
        var CepAbertoApi = new CepAbertoApi(settings.CepAbertoToken, client, cash);

        var options = AppDbContext.CreateOptions(settings);

        var DbContext = new AppDbContext(options);
        var UserRepository = new UserRepository(DbContext);
        var DistanceRepository = new DistanceRepository(DbContext);

        UserService = new UserService(UserRepository, AuthDependence);
        DistanceService = new DistanceService(DistanceRepository, CepAbertoApi);
        TokenService = new TokenService(UserRepository, TokenDependence);
    }
}