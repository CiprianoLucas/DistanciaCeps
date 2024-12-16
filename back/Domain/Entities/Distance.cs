namespace Back.Domain.Entities;

public class Distance
{
    public int Id { get; set; }
    public required string De { get; set; }
    public required string Para { get; set; }
    public required float Distancia { get; set; }
    public required User User { get; set; }

    public required int UserId { get; set; }
}