using BackendPro.Core.DTOs;

namespace BackendPro.Web.Models;

public class GeneroDetailsViewModel
{
    public GeneroDto Genero { get; set; } = new();
    public IReadOnlyList<PeliculaSummaryDto> Peliculas { get; set; } = Array.Empty<PeliculaSummaryDto>();
}
