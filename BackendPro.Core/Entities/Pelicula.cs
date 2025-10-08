namespace BackendPro.Core.Entities;

public class Pelicula
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Sinopsis { get; set; } = string.Empty;

    public int Duracion { get; set; } // Duration in minutes

    public DateTime FechaEstreno { get; set; }

    public string? ImagenUrl { get; set; }

    public int GeneroId { get; set; }
    public virtual Genero Genero { get; set; } = null!;

    public int DirectorId { get; set; }
    public virtual Director Director { get; set; } = null!;

    public virtual ICollection<PeliculaActor> PeliculasActor { get; set; } = [];
}
