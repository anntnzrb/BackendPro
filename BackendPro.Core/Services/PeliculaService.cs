using Microsoft.Extensions.Logging;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Core.Exceptions;
using BackendPro.Core.Interfaces;

namespace BackendPro.Core.Services;

public class PeliculaService(IUnitOfWork unitOfWork, ILogger<PeliculaService> logger) : IPeliculaService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<PeliculaService> _logger = logger;

    public async Task<IEnumerable<PeliculaDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var peliculas = await _unitOfWork.Peliculas.GetAllWithDetailsAsync(cancellationToken).ConfigureAwait(false);
        return peliculas.Select(MapToDto);
    }

    public async Task<IEnumerable<PeliculaDto>> GetFilteredAsync(int? generoId, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var peliculas = await _unitOfWork.Peliculas.GetFilteredWithDetailsAsync(generoId, searchTerm, cancellationToken).ConfigureAwait(false);
        return peliculas.Select(MapToDto);
    }

    public async Task<IEnumerable<PeliculaDto>> GetByDirectorIdAsync(int directorId, CancellationToken cancellationToken = default)
    {
        var peliculas = await _unitOfWork.Peliculas.GetByDirectorIdWithDetailsAsync(directorId, cancellationToken).ConfigureAwait(false);
        return peliculas.Select(MapToDto);
    }

    public async Task<PeliculaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var pelicula = await _unitOfWork.Peliculas.GetByIdWithDetailsAsync(id, cancellationToken).ConfigureAwait(false);
        return pelicula == null ? null : MapToDto(pelicula);
    }

    public async Task<PeliculaDto> CreateAsync(CreatePeliculaDto createDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating película with title {PeliculaTitulo}", createDto.Titulo);

        var pelicula = new Pelicula
        {
            Titulo = createDto.Titulo,
            Sinopsis = createDto.Sinopsis,
            Duracion = createDto.Duracion,
            FechaEstreno = createDto.FechaEstreno,
            ImagenUrl = string.IsNullOrWhiteSpace(createDto.ImagenUrl) ? null : createDto.ImagenUrl,
            GeneroId = createDto.GeneroId,
            DirectorId = createDto.DirectorId,
            PeliculasActor = createDto.ActorIds
                .Distinct()
                .Select(actorId => new PeliculaActor { ActorId = actorId })
                .ToList(),
        };

        await _unitOfWork.Peliculas.AddAsync(pelicula, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var createdPelicula = await _unitOfWork.Peliculas.GetByIdWithDetailsAsync(pelicula.Id, cancellationToken).ConfigureAwait(false);
        return MapToDto(createdPelicula!);
    }

    public async Task<PeliculaDto?> UpdateAsync(int id, UpdatePeliculaDto updateDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating película with ID {PeliculaId}", id);

        var pelicula = await _unitOfWork.Peliculas.GetByIdWithDetailsAsync(id, cancellationToken).ConfigureAwait(false);
        if (pelicula == null)
        {
            _logger.LogWarning("Película with ID {PeliculaId} not found for update", id);
            throw new EntityNotFoundException(nameof(Pelicula), id);
        }

        pelicula.Titulo = updateDto.Titulo;
        pelicula.Sinopsis = updateDto.Sinopsis;
        pelicula.Duracion = updateDto.Duracion;
        pelicula.FechaEstreno = updateDto.FechaEstreno;
        pelicula.ImagenUrl = string.IsNullOrWhiteSpace(updateDto.ImagenUrl) ? null : updateDto.ImagenUrl;
        pelicula.GeneroId = updateDto.GeneroId;
        pelicula.DirectorId = updateDto.DirectorId;

        var selectedActorIds = updateDto.ActorIds.Distinct().ToList();
        var currentActorIds = pelicula.PeliculasActor.Select(pa => pa.ActorId).ToList();

        var actorsToRemove = pelicula.PeliculasActor.Where(pa => !selectedActorIds.Contains(pa.ActorId)).ToList();
        foreach (var peliculaActor in actorsToRemove)
        {
            pelicula.PeliculasActor.Remove(peliculaActor);
        }

        foreach (var actorId in selectedActorIds)
        {
            if (!currentActorIds.Contains(actorId))
            {
                pelicula.PeliculasActor.Add(new PeliculaActor { ActorId = actorId, PeliculaId = pelicula.Id });
            }
        }

        _unitOfWork.Peliculas.Update(pelicula);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var updatedPelicula = await _unitOfWork.Peliculas.GetByIdWithDetailsAsync(id, cancellationToken).ConfigureAwait(false);
        return MapToDto(updatedPelicula!);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var pelicula = await _unitOfWork.Peliculas.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (pelicula == null)
        {
            return false;
        }

        _unitOfWork.Peliculas.Remove(pelicula);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    private static PeliculaDto MapToDto(Pelicula pelicula)
    {
        return new PeliculaDto
        {
            Id = pelicula.Id,
            Titulo = pelicula.Titulo,
            Sinopsis = pelicula.Sinopsis,
            Duracion = pelicula.Duracion,
            FechaEstreno = pelicula.FechaEstreno,
            ImagenUrl = pelicula.ImagenUrl,
            Genero = new GeneroDto
            {
                Id = pelicula.Genero.Id,
                Nombre = pelicula.Genero.Nombre,
                Descripcion = pelicula.Genero.Descripcion,
            },
            Director = new DirectorDto
            {
                Id = pelicula.Director.Id,
                Nombre = pelicula.Director.Nombre,
                Nacionalidad = pelicula.Director.Nacionalidad,
                FechaNacimiento = pelicula.Director.FechaNacimiento,
            },
            Actores = pelicula.PeliculasActor
                .Where(pa => pa.Actor != null)
                .Select(pa => new ActorDto
                {
                    Id = pa.Actor.Id,
                    Nombre = pa.Actor.Nombre,
                    Biografia = pa.Actor.Biografia,
                    FechaNacimiento = pa.Actor.FechaNacimiento,
                })
                .ToList(),
        };
    }
}
