using BackendPro.Core.Entities;

namespace BackendPro.Core.Interfaces;

public interface IResenaRepository : IGenericRepository<Resena>
{
    Task<IEnumerable<Resena>> GetByPeliculaIdAsync(int peliculaId, CancellationToken cancellationToken = default);
}
