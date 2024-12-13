using System.ComponentModel.DataAnnotations;

namespace Back.Domain.Entities;
public class Local
{
    [Key]
    public int Id { get; set; }
    public required string Cep { get; set; }
    public required decimal Latitude { get; set; }
    public required decimal Longitude { get; set; }
}