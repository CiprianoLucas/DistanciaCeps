namespace Back.Infra.API;

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
        float latitude = 1;
        float longitude = 2;
        response.EnsureSuccessStatusCode();
        await response.Content.ReadAsStringAsync();

        return (latitude, longitude);
    }

}