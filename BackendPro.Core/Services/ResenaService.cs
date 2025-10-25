using Microsoft.Extensions.Logging;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Core.Exceptions;
using BackendPro.Core.Interfaces;

namespace BackendPro.Core.Services;

public class ResenaService(IUnitOfWork unitOfWork, ILogger<ResenaService> logger) : IResenaService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ResenaService> _logger = logger;

    public async Task<IEnumerable<ResenaDto>> GetByPeliculaIdAsync(int peliculaId, CancellationToken cancellationToken = default)
    {
        var resenas = await _unitOfWork.Resenas.GetByPeliculaIdAsync(peliculaId, cancellationToken).ConfigureAwait(false);
        return resenas.Select(MapToDto);
    }

    public async Task<ResenaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var resena = await _unitOfWork.Resenas.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        return resena == null ? null : MapToDto(resena);
    }

    public async Task<ResenaDto> CreateAsync(CreateResenaDto createResenaDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating resena for película {PeliculaId} by {Autor}", createResenaDto.PeliculaId, createResenaDto.Autor);

        // Validate that the película exists
        var pelicula = await _unitOfWork.Peliculas.GetByIdAsync(createResenaDto.PeliculaId, cancellationToken).ConfigureAwait(false);
        if (pelicula == null)
        {
            throw new EntityNotFoundException($"Película with ID {createResenaDto.PeliculaId} not found");
        }

        // Validate rating is within bounds
        if (createResenaDto.Calificacion is < 1 or > 5)
        {
            throw new ArgumentException("Calificación must be between 1 and 5", nameof(createResenaDto));
        }

        // Validate comment is not empty
        if (string.IsNullOrWhiteSpace(createResenaDto.Comentario))
        {
            throw new ArgumentException("Comentario cannot be empty", nameof(createResenaDto));
        }

        var resena = new Resena
        {
            Autor = createResenaDto.Autor.Trim(),
            Comentario = createResenaDto.Comentario.Trim(),
            Calificacion = createResenaDto.Calificacion,
            PeliculaId = createResenaDto.PeliculaId,
            FechaPublicacion = DateTime.UtcNow,
        };

        await _unitOfWork.Resenas.AddAsync(resena, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(resena);
    }

    public async Task<ResenaDto> UpdateAsync(int id, UpdateResenaDto updateResenaDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating resena {ResenaId} by {Autor}", id, updateResenaDto.Autor);

        var resena = await _unitOfWork.Resenas.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (resena == null)
        {
            throw new EntityNotFoundException($"Resena with ID {id} not found");
        }

        // Validate that the película exists
        var pelicula = await _unitOfWork.Peliculas.GetByIdAsync(updateResenaDto.PeliculaId, cancellationToken).ConfigureAwait(false);
        if (pelicula == null)
        {
            throw new EntityNotFoundException($"Película with ID {updateResenaDto.PeliculaId} not found");
        }

        // Validate rating is within bounds
        if (updateResenaDto.Calificacion is < 1 or > 5)
        {
            throw new ArgumentException("Calificación must be between 1 and 5", nameof(updateResenaDto));
        }

        // Validate comment is not empty
        if (string.IsNullOrWhiteSpace(updateResenaDto.Comentario))
        {
            throw new ArgumentException("Comentario cannot be empty", nameof(updateResenaDto));
        }

        resena.Autor = updateResenaDto.Autor.Trim();
        resena.Comentario = updateResenaDto.Comentario.Trim();
        resena.Calificacion = updateResenaDto.Calificacion;
        resena.PeliculaId = updateResenaDto.PeliculaId;

        _unitOfWork.Resenas.Update(resena);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(resena);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var resena = await _unitOfWork.Resenas.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (resena == null)
        {
            return false;
        }

        _unitOfWork.Resenas.Remove(resena);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    public async Task<double> GetAverageRatingAsync(int peliculaId, CancellationToken cancellationToken = default)
    {
        var resenas = await _unitOfWork.Resenas.GetByPeliculaIdAsync(peliculaId, cancellationToken).ConfigureAwait(false);
        return resenas.Any() ? resenas.Average(r => r.Calificacion) : 0;
    }

    public async Task<int> GetReviewCountAsync(int peliculaId, CancellationToken cancellationToken = default)
    {
        var resenas = await _unitOfWork.Resenas.GetByPeliculaIdAsync(peliculaId, cancellationToken).ConfigureAwait(false);
        return resenas.Count();
    }

    private static ResenaDto MapToDto(Resena resena)
    {
        return new ResenaDto
        {
            Id = resena.Id,
            Autor = resena.Autor,
            Comentario = resena.Comentario,
            Calificacion = resena.Calificacion,
            FechaPublicacion = resena.FechaPublicacion,
            PeliculaId = resena.PeliculaId,
        };
    }
}
