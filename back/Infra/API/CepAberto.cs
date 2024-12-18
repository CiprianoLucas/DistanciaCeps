namespace Back.Infra.API;
using System.Text.Json;
using System.Net.Http;
using Back.Infra.Cash;
public class JsonCepAbertoResponse
{
    public required string latitude { get; set; }
    public required string longitude { get; set; }
}

public class CepAbertoApi
{

    private const string Url = "https://www.cepaberto.com/api/v3/";
    private string Token { get; set; }
    private Cash Cash { get; set; }

    private HttpClient Client;

    public CepAbertoApi(string token, HttpClient client, Cash cash)
    {
        Token = $"Token token={token}";
        Client = client;
        Cash = cash;
    }



    public async Task<(float Latitude, float Longitude, bool cashed)> GetLatAndLongAsync(string cep, bool wait = false)
    {
        var cashCep = await VerifyCashCep(cep);
        if (cashCep.HasValue)
        {
            var (Latitude, Longitude) = cashCep.Value;
            return (Latitude, Longitude, true);
        }
        if (wait)
        {
            await Task.Delay(1000);
        }
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Url}cep?cep={cep}");
        request.Headers.Add("Authorization", Token);
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var data = JsonSerializer.Deserialize<JsonCepAbertoResponse>(content, options);
        if (data == null || string.IsNullOrEmpty(data.latitude) || string.IsNullOrEmpty(data.longitude))
        {
            throw new KeyNotFoundException("Dados inválidos ou CEP não encontrado.");
        }
        float latitude = float.Parse((string)data.latitude);
        float longitude = float.Parse((string)data.longitude);
        ToCashCep(cep, latitude, longitude);
        return (latitude, longitude, false);
    }

    protected async Task<(float Latitude, float Longitude)?> VerifyCashCep(string cep)
    {
        string? cashCep = await Cash.Get(cep);
        if (cashCep == null) return null;

        string[] parts = cashCep.Split(':');
        if (parts.Length != 2) return null;

        var latitude = float.Parse(parts[0]);
        var longitude = float.Parse(parts[1]);
        return (latitude, longitude);
    }

    protected void ToCashCep(string cep, float latitude, float longitude)
    {
        string newCash = string.Join(":", latitude, longitude);
        Cash.Set(cep, newCash);
    }
}