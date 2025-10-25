namespace BackendPro.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGeneroRepository Generos { get; }
    IActorRepository Actores { get; }
    IDirectorRepository Directores { get; }
    IPeliculaRepository Peliculas { get; }
    IResenaRepository Resenas { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
