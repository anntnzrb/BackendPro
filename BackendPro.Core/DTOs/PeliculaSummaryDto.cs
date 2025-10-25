namespace BackendPro.Core.DTOs;

public class PeliculaSummaryDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaEstreno { get; set; }
    public GeneroDto Genero { get; set; } = null!;
}
