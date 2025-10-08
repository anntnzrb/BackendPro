namespace BackendPro.Core.DTOs;

public class UpdatePeliculaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public int Duracion { get; set; }
    public DateTime FechaEstreno { get; set; }
    public string? ImagenUrl { get; set; }
    public int GeneroId { get; set; }
    public int DirectorId { get; set; }
    public ICollection<int> ActorIds { get; set; } = [];
}
