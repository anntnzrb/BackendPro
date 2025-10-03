using BackendPro.Core.DTOs;

namespace BackendPro.Web.Models;

public class GeneroDetailsViewModel
{
    public GeneroDto Genero { get; set; } = new();
    public IReadOnlyList<PeliculaSummaryViewModel> Peliculas { get; set; } = Array.Empty<PeliculaSummaryViewModel>();
}
