using BackendPro.Core.DTOs;

namespace BackendPro.Core.Interfaces;

public interface IDirectorService
{
    Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DirectorDto?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default);
    Task<DirectorDto> CreateAsync(CreateDirectorDto createDto, CancellationToken cancellationToken = default);
    Task<DirectorDto?> UpdateAsync(int id, UpdateDirectorDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default);
}
