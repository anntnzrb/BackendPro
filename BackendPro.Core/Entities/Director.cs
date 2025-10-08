namespace BackendPro.Core.Entities;

public class Director
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Nacionalidad { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public virtual ICollection<Pelicula> Peliculas { get; set; } = [];
}
