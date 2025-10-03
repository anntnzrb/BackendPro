namespace BackendPro.Core.Entities;

public class Actor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Biografia { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public virtual ICollection<PeliculaActor> PeliculasActor { get; set; } = [];
}
