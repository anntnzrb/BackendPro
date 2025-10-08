using BackendPro.Core.DTOs;

namespace BackendPro.Web.Models;

public class PeliculaSummaryViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaEstreno { get; set; }
    public string Genero { get; set; } = string.Empty;
}

public class ActorDetailsViewModel
{
    public ActorDto Actor { get; set; } = new();
    public List<PeliculaSummaryViewModel> Peliculas { get; set; } = new();
}

public class DirectorDetailsViewModel
{
    public DirectorDto Director { get; set; } = new();
    public List<PeliculaSummaryViewModel> Peliculas { get; set; } = new();
}

public class GeneroDetailsViewModel
{
    public GeneroDto Genero { get; set; } = new();
    public List<PeliculaSummaryViewModel> Peliculas { get; set; } = new();
}
