using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;
using BackendPro.Core.Interfaces;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Infrastructure.Repositories;

public class GeneroRepository(ApplicationDbContext context) : GenericRepository<Genero>(context), IGeneroRepository
{
    public async Task<Genero?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(g => g.Peliculas)
                .ThenInclude(p => p.Director)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Peliculas
            .AnyAsync(p => p.GeneroId == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
