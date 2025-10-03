using BackendPro.Core.Entities;

namespace BackendPro.Core.Interfaces;

public interface IDirectorRepository : IGenericRepository<Director>
{
    Task<Director?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default);
}
