using Microsoft.Extensions.Logging;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Core.Exceptions;
using BackendPro.Core.Interfaces;

namespace BackendPro.Core.Services;

public class GeneroService(IUnitOfWork unitOfWork, ILogger<GeneroService> logger) : IGeneroService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<GeneroService> _logger = logger;

    public async Task<IEnumerable<GeneroDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var generos = await _unitOfWork.Generos.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return generos.Select(MapToDto);
    }

    public async Task<GeneroDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting género with ID {GeneroId}", id);
        var genero = await _unitOfWork.Generos.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (genero == null)
        {
            _logger.LogWarning("Género with ID {GeneroId} not found", id);
            return null;
        }

        return MapToDto(genero);
    }

    public async Task<GeneroDto?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        var genero = await _unitOfWork.Generos.GetByIdWithPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
        return genero == null ? null : MapToDto(genero);
    }

    public async Task<GeneroDto> CreateAsync(CreateGeneroDto createDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating género with name {GeneroNombre}", createDto.Nombre);

        var genero = new Genero
        {
            Nombre = createDto.Nombre,
            Descripcion = createDto.Descripcion,
        };

        await _unitOfWork.Generos.AddAsync(genero, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Género created successfully with ID {GeneroId}", genero.Id);
        return MapToDto(genero);
    }

    public async Task<GeneroDto?> UpdateAsync(int id, UpdateGeneroDto updateDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating género with ID {GeneroId}", id);

        var genero = await _unitOfWork.Generos.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (genero == null)
        {
            _logger.LogWarning("Género with ID {GeneroId} not found for update", id);
            throw new EntityNotFoundException(nameof(Genero), id);
        }

        genero.Nombre = updateDto.Nombre;
        genero.Descripcion = updateDto.Descripcion;

        _unitOfWork.Generos.Update(genero);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Género with ID {GeneroId} updated successfully", id);
        return MapToDto(genero);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting género with ID {GeneroId}", id);

        var genero = await _unitOfWork.Generos.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (genero == null)
        {
            _logger.LogWarning("Género with ID {GeneroId} not found for deletion", id);
            throw new EntityNotFoundException(nameof(Genero), id);
        }

        var hasPeliculas = await _unitOfWork.Generos.HasPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
        if (hasPeliculas)
        {
            _logger.LogWarning("Cannot delete género with ID {GeneroId} because it has associated películas", id);
            throw new BusinessValidationException("No se puede eliminar el género porque tiene películas asociadas.");
        }

        _unitOfWork.Generos.Remove(genero);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Género with ID {GeneroId} deleted successfully", id);
        return true;
    }

    public async Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Generos.HasPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
    }

    private static GeneroDto MapToDto(Genero genero)
    {
        return new GeneroDto
        {
            Id = genero.Id,
            Nombre = genero.Nombre,
            Descripcion = genero.Descripcion,
        };
    }
}
