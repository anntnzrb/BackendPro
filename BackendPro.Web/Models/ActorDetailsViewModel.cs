using BackendPro.Core.DTOs;

namespace BackendPro.Web.Models;

public class ActorDetailsViewModel
{
    public ActorDto Actor { get; set; } = new();
    public IReadOnlyList<PeliculaSummaryViewModel> Peliculas { get; set; } = Array.Empty<PeliculaSummaryViewModel>();
}
