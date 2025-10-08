namespace BackendPro.Core.Entities;

public class Genero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public virtual ICollection<Pelicula> Peliculas { get; set; } = [];
}
