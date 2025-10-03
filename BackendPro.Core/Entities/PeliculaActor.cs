namespace BackendPro.Core.Entities;

public class PeliculaActor
{
    public int PeliculaId { get; set; }
    public virtual Pelicula Pelicula { get; set; } = null!;

    public int ActorId { get; set; }
    public virtual Actor Actor { get; set; } = null!;
}
