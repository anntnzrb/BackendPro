using BackendPro.Core.Entities;

namespace BackendPro.Core.Interfaces;

public interface IGeneroRepository : IGenericRepository<Genero>
{
    Task<Genero?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default);
}
