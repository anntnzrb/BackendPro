using Microsoft.Extensions.Logging;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Core.Exceptions;
using BackendPro.Core.Interfaces;

namespace BackendPro.Core.Services;

public class DirectorService(IUnitOfWork unitOfWork, ILogger<DirectorService> logger) : IDirectorService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DirectorService> _logger = logger;

    public async Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var directores = await _unitOfWork.Directores.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return directores.Select(MapToDto);
    }

    public async Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var director = await _unitOfWork.Directores.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        return director == null ? null : MapToDto(director);
    }

    public async Task<DirectorDto?> GetByIdWithPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        var director = await _unitOfWork.Directores.GetByIdWithPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
        return director == null ? null : MapToDto(director);
    }

    public async Task<DirectorDto> CreateAsync(CreateDirectorDto createDto, CancellationToken cancellationToken = default)
    {
        var director = new Director
        {
            Nombre = createDto.Nombre,
            Nacionalidad = createDto.Nacionalidad,
            FechaNacimiento = createDto.FechaNacimiento,
        };

        await _unitOfWork.Directores.AddAsync(director, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(director);
    }

    public async Task<DirectorDto?> UpdateAsync(int id, UpdateDirectorDto updateDto, CancellationToken cancellationToken = default)
    {
        var director = await _unitOfWork.Directores.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (director == null)
        {
            return null;
        }

        director.Nombre = updateDto.Nombre;
        director.Nacionalidad = updateDto.Nacionalidad;
        director.FechaNacimiento = updateDto.FechaNacimiento;

        _unitOfWork.Directores.Update(director);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(director);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting director with ID {DirectorId}", id);

        var director = await _unitOfWork.Directores.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (director == null)
        {
            _logger.LogWarning("Director with ID {DirectorId} not found for deletion", id);
            throw new EntityNotFoundException(nameof(Director), id);
        }

        var hasPeliculas = await _unitOfWork.Directores.HasPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
        if (hasPeliculas)
        {
            _logger.LogWarning("Cannot delete director with ID {DirectorId} because it has associated películas", id);
            throw new BusinessValidationException("No se puede eliminar el director porque tiene películas asociadas.");
        }

        _unitOfWork.Directores.Remove(director);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Director with ID {DirectorId} deleted successfully", id);
        return true;
    }

    public async Task<bool> HasPeliculasAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Directores.HasPeliculasAsync(id, cancellationToken).ConfigureAwait(false);
    }

    private static DirectorDto MapToDto(Director director)
    {
        return new DirectorDto
        {
            Id = director.Id,
            Nombre = director.Nombre,
            Nacionalidad = director.Nacionalidad,
            FechaNacimiento = director.FechaNacimiento,
        };
    }
}
