namespace Back.App.DTOs;


public class DistanceDto
{
    public required string De { get; set; }
    public required string Para { get; set; }
}

public class DistanceResponseDto
{
    public string De { get; set; }
    public string Para { get; set; }
    public float Distancia { get; set; }

    public DistanceResponseDto(string de, string para, float distancia)
    {
        De = de;
        Para = para;
        Distancia = distancia;
    }

    public static DistanceResponseDto Create(string de, string para, float distancia)
    {
        return new DistanceResponseDto(de, para, distancia);
    }

}