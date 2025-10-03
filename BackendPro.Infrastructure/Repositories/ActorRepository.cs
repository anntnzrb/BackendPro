using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;
using BackendPro.Core.Interfaces;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Infrastructure.Repositories;

public class ActorRepository(ApplicationDbContext context) : GenericRepository<Actor>(context), IActorRepository
{
    public async Task<Actor?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(a => a.PeliculasActor)
                .ThenInclude(pa => pa.Pelicula)
                    .ThenInclude(p => p.Genero)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
