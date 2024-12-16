namespace Back.App.DTOs;


public class DistanceDto
{
    public required string De { get; set; }
    public required string Para { get; set; }
}

public class DistanceResponseDto
{
    public string De;
    public string Para;
    public float Distancia;

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