namespace Back.Domain.Entities;

public class Distance
{
    public required Local De { get; set; }
    public required Local Para { get; set; }
    public float Distancia { get; }
}