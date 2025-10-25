using BackendPro.Core.DTOs;

namespace BackendPro.Core.Interfaces;

public interface IResenaService
{
    Task<IEnumerable<ResenaDto>> GetByPeliculaIdAsync(int peliculaId, CancellationToken cancellationToken = default);
    Task<ResenaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ResenaDto> CreateAsync(CreateResenaDto createResenaDto, CancellationToken cancellationToken = default);
    Task<ResenaDto> UpdateAsync(int id, UpdateResenaDto updateResenaDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingAsync(int peliculaId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountAsync(int peliculaId, CancellationToken cancellationToken = default);
}
