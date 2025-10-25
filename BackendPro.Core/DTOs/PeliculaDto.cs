namespace BackendPro.Core.DTOs;

public class PeliculaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public int Duracion { get; set; }
    public DateTime FechaEstreno { get; set; }
    public string? ImagenUrl { get; set; }
    public GeneroDto Genero { get; set; } = null!;
    public DirectorDto Director { get; set; } = null!;
    public ICollection<ActorDto> Actores { get; set; } = [];
    public ICollection<ResenaDto> Resenas { get; set; } = [];
}
