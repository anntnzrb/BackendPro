using BackendPro.Core.Entities;

namespace BackendPro.Core.Interfaces;

public interface IActorRepository : IGenericRepository<Actor>
{
    Task<Actor?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default);
}
