namespace BackendPro.Core.Entities;

public class Resena
{
    public int Id { get; set; }

    public int PeliculaId { get; set; }
    public virtual Pelicula Pelicula { get; set; } = null!;

    public string Autor { get; set; } = string.Empty;

    public string Comentario { get; set; } = string.Empty;

    public int Calificacion { get; set; } // 1-5

    public DateTime FechaPublicacion { get; set; }
}
