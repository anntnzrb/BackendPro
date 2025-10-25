using Microsoft.Extensions.Logging;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Core.Exceptions;
using BackendPro.Core.Interfaces;

namespace BackendPro.Core.Services;

public class ActorService(IUnitOfWork unitOfWork, ILogger<ActorService> logger) : IActorService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ActorService> _logger = logger;

    public async Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var actores = await _unitOfWork.Actores.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return actores.Select(MapToDto);
    }

    public async Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var actor = await _unitOfWork.Actores.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        return actor == null ? null : MapToDto(actor);
    }

    public async Task<ActorDto?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        var actor = await _unitOfWork.Actores.GetByIdWithPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
        return actor == null ? null : MapToDto(actor);
    }

    public async Task<ActorDto> CreateAsync(CreateActorDto createDto, CancellationToken cancellationToken = default)
    {
        var actor = new Actor
        {
            Nombre = createDto.Nombre,
            Biografia = createDto.Biografia,
            FechaNacimiento = createDto.FechaNacimiento,
        };

        await _unitOfWork.Actores.AddAsync(actor, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(actor);
    }

    public async Task<ActorDto?> UpdateAsync(int id, UpdateActorDto updateDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating actor with ID {ActorId}", id);

        var actor = await _unitOfWork.Actores.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (actor == null)
        {
            _logger.LogWarning("Actor with ID {ActorId} not found for update", id);
            throw new EntityNotFoundException(nameof(Actor), id);
        }

        actor.Nombre = updateDto.Nombre;
        actor.Biografia = updateDto.Biografia;
        actor.FechaNacimiento = updateDto.FechaNacimiento;

        _unitOfWork.Actores.Update(actor);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Actor with ID {ActorId} updated successfully", id);
        return MapToDto(actor);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var actor = await _unitOfWork.Actores.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (actor == null)
        {
            return false;
        }

        _unitOfWork.Actores.Remove(actor);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    private static ActorDto MapToDto(Actor actor)
    {
        return new ActorDto
        {
            Id = actor.Id,
            Nombre = actor.Nombre,
            Biografia = actor.Biografia,
            FechaNacimiento = actor.FechaNacimiento,
            Peliculas = actor.PeliculasActor?.Select(pa => new PeliculaSummaryDto
            {
                Id = pa.Pelicula.Id,
                Titulo = pa.Pelicula.Titulo,
                FechaEstreno = pa.Pelicula.FechaEstreno,
                Genero = new GeneroDto { Id = pa.Pelicula.Genero.Id, Nombre = pa.Pelicula.Genero.Nombre },
            }).ToList() ?? [],
        };
    }
}
