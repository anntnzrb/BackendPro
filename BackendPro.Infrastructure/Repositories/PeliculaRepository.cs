using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;
using BackendPro.Core.Interfaces;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Infrastructure.Repositories;

public class PeliculaRepository(ApplicationDbContext context) : GenericRepository<Pelicula>(context), IPeliculaRepository
{
    public async Task<Pelicula?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .Include(p => p.Resenas)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Pelicula>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .Include(p => p.Resenas)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Pelicula>> GetFilteredWithDetailsAsync(int? generoId, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .Include(p => p.Resenas)
            .AsQueryable();

        if (generoId.HasValue)
        {
            query = query.Where(p => p.GeneroId == generoId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Titulo.Contains(searchTerm));
        }

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Pelicula>> GetByDirectorIdWithDetailsAsync(int directorId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.DirectorId == directorId)
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .Include(p => p.Resenas)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
