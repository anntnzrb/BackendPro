using BackendPro.Core.DTOs;

namespace BackendPro.Core.Interfaces;

public interface IPeliculaService
{
    Task<IEnumerable<PeliculaDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PeliculaDto>> GetFilteredAsync(int? generoId, string? searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<PeliculaDto>> GetByDirectorIdAsync(int directorId, CancellationToken cancellationToken = default);
    Task<PeliculaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PeliculaDto> CreateAsync(CreatePeliculaDto createDto, CancellationToken cancellationToken = default);
    Task<PeliculaDto?> UpdateAsync(int id, UpdatePeliculaDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
