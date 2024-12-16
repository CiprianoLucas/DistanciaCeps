using Back.Domain.Entities;
using Back.Infra.Database;
using Back.Infra.API;


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
    public async Task<Distance[]> GetDistancesByUserAsync(User user)
    {
        return await _DistanceRepository.ListByUserAsync(user);
    }

    public async Task<Distance> GetCalculateAndSaveAsync(string de, string para, User user)
    {
        var (DeLat, DeLong) = await GetCoordByCep(de);
        var (ParaLat, ParaLong) = await GetCoordByCep(para);
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

        return Distance;

    }
    public async Task<(float, float)> GetCoordByCep(string cep)
    {
        return await _CepAbertoApi.GetLatAndLongAsync(cep);
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