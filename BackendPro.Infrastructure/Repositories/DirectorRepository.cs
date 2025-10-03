using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;
using BackendPro.Core.Interfaces;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Infrastructure.Repositories;

public class DirectorRepository(ApplicationDbContext context) : GenericRepository<Director>(context), IDirectorRepository
{
    public async Task<Director?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(d => d.Peliculas)
                .ThenInclude(p => p.Genero)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Peliculas
            .AnyAsync(p => p.DirectorId == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
