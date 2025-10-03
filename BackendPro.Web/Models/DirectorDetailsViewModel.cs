using BackendPro.Core.DTOs;

namespace BackendPro.Web.Models;

public class DirectorDetailsViewModel
{
    public DirectorDto Director { get; set; } = new();
    public IReadOnlyList<PeliculaSummaryViewModel> Peliculas { get; set; } = Array.Empty<PeliculaSummaryViewModel>();
}
