using BackendPro.Core.DTOs;

namespace BackendPro.Core.Interfaces;

public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ActorDto?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default);
    Task<ActorDto> CreateAsync(CreateActorDto createDto, CancellationToken cancellationToken = default);
    Task<ActorDto?> UpdateAsync(int id, UpdateActorDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
