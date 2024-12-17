namespace Back.Infra.API;
using System.Text.Json;
using System.Net.Http;

public class JsonCepAbertoResponse
{
    public required string latitude { get; set; }
    public required string longitude { get; set; }
}

public class CepAbertoApi
{

    private const string Url = "https://www.cepaberto.com/api/v3/";
    private string Token { get; set; }

    private HttpClient client = new HttpClient();

    public CepAbertoApi(string token)
    {
        Token = $"Token token={token}";
    }



    public async Task<(float Latitude, float Longitude)> GetLatAndLongAsync(string cep)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Url}cep?cep={cep}");
        request.Headers.Add("Authorization", Token);
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        try
        {
            var data = JsonSerializer.Deserialize<JsonCepAbertoResponse>(content, options);
            float latitude = float.Parse((string)data.latitude);
            float longitude = float.Parse((string)data.longitude);
            return (latitude, longitude);
        }
        catch
        {
            throw new KeyNotFoundException("CEP não encontrado: " + cep);
        }

    }

}