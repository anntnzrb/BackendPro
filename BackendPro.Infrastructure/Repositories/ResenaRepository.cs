using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;
using BackendPro.Core.Interfaces;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Infrastructure.Repositories;

public class ResenaRepository(ApplicationDbContext context) : GenericRepository<Resena>(context), IResenaRepository
{
    public async Task<IEnumerable<Resena>> GetByPeliculaIdAsync(int peliculaId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(r => r.PeliculaId == peliculaId)
            .OrderByDescending(r => r.FechaPublicacion)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
