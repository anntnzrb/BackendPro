using BackendPro.Core.Interfaces;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Infrastructure.Repositories;

public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private IGeneroRepository? _generos;
    private IActorRepository? _actores;
    private IDirectorRepository? _directores;
    private IPeliculaRepository? _peliculas;
    private IResenaRepository? _resenas;
    private bool _disposed;

    public IGeneroRepository Generos => _generos ??= new GeneroRepository(_context);
    public IActorRepository Actores => _actores ??= new ActorRepository(_context);
    public IDirectorRepository Directores => _directores ??= new DirectorRepository(_context);
    public IPeliculaRepository Peliculas => _peliculas ??= new PeliculaRepository(_context);
    public IResenaRepository Resenas => _resenas ??= new ResenaRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
