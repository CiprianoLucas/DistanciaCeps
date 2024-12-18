using Back.Domain.Entities;
using Back.Infra.Database;
using Back.Infra.API;
using Back.Domain.Validations;

namespace Back.Domain.Services;
public class DistanceService
{
    private readonly DistanceRepository _DistanceRepository;
    private readonly CepAbertoApi _CepAbertoApi;

    public DistanceService(DistanceRepository repository, CepAbertoApi cepAbertoApi)
    {
        _DistanceRepository = repository;
        _CepAbertoApi = cepAbertoApi;
    }
    public async Task<Distance[]> GetDistancesByUserAsync(User user, string? de, string? para)
    {
        string deValidated = CepValidator.Formate(de);
        string paraValidated = CepValidator.Formate(para);
        return await _DistanceRepository.ListByUserAsync(user, deValidated, paraValidated);
    }

    public async Task<(Distance, bool)> GetCalculateAndSaveAsync(string de, string para, User user)
    {
        de = CepValidator.Formate(de);
        para = CepValidator.Formate(para);
        if (de == "" || para == "")
        {
            throw new ArgumentNullException("Cep não pode estar em branco");
        }
        var (DeLat, DeLong, cashedDe) = await GetCoordByCep(de);
        var (ParaLat, ParaLong, cashedPara) = await GetCoordByCep(para, !cashedDe);
        float distance = Calculate(DeLat, DeLong, ParaLat, ParaLong);

        var Distance = new Distance
        {
            De = de,
            Para = para,
            Distancia = distance,
            User = user,
            UserId = user.Id
        };
        Distance = await _DistanceRepository.AddOrUpdateAsync(Distance);
        bool cached = cashedDe & cashedPara;
        return (Distance, cached);

    }
    public async Task<(float, float, bool)> GetCoordByCep(string cep, bool wait = false)
    {
        return await _CepAbertoApi.GetLatAndLongAsync(cep, wait);
    }
    public float Calculate(float DeLat, float DeLong, float ParaLat, float ParaLong)
    {
        const int EartgRadiusKm = 6371;
        double dLat = ToRadians(DeLat - ParaLat);
        double dLon = ToRadians(DeLong - ParaLong);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(DeLat)) * Math.Cos(ToRadians(ParaLat)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = (float)(EartgRadiusKm * c);
        return distance;
    }
    private double ToRadians(double angle)
    {
        return angle * Math.PI / 180;
    }
}