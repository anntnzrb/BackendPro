using BackendPro.Core.DTOs;

namespace BackendPro.Core.Interfaces;

public interface IGeneroService
{
    Task<IEnumerable<GeneroDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GeneroDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GeneroDto?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default);
    Task<GeneroDto> CreateAsync(CreateGeneroDto createDto, CancellationToken cancellationToken = default);
    Task<GeneroDto?> UpdateAsync(int id, UpdateGeneroDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default);
}
