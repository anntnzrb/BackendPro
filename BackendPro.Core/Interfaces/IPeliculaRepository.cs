using BackendPro.Core.Entities;

namespace BackendPro.Core.Interfaces;

public interface IPeliculaRepository : IGenericRepository<Pelicula>
{
    Task<Pelicula?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pelicula>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Pelicula>> GetFilteredWithDetailsAsync(int? generoId, string? searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pelicula>> GetByDirectorIdWithDetailsAsync(int directorId, CancellationToken cancellationToken = default);
}
