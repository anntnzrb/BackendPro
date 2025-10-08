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
    public List<ActorDto> Actores { get; set; } = new();
}

public class CreatePeliculaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public int Duracion { get; set; }
    public DateTime FechaEstreno { get; set; }
    public string? ImagenUrl { get; set; }
    public int GeneroId { get; set; }
    public int DirectorId { get; set; }
    public List<int> ActorIds { get; set; } = new();
}

public class UpdatePeliculaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public int Duracion { get; set; }
    public DateTime FechaEstreno { get; set; }
    public string? ImagenUrl { get; set; }
    public int GeneroId { get; set; }
    public int DirectorId { get; set; }
    public List<int> ActorIds { get; set; } = new();
}
