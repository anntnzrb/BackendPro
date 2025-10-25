namespace BackendPro.Core.DTOs;

public class ActorDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public ICollection<PeliculaSummaryDto> Peliculas { get; set; } = [];
}
