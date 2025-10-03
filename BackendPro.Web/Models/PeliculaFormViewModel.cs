using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendPro.Web.Models;

public class PeliculaFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La sinopsis es obligatoria.")]
    [Display(Name = "Sinopsis")]
    [StringLength(2000, ErrorMessage = "La sinopsis no puede superar los 2000 caracteres.")]
    public string Sinopsis { get; set; } = string.Empty;

    [Required(ErrorMessage = "La duración es obligatoria.")]
    [Range(1, 600, ErrorMessage = "La duración debe estar entre 1 y 600 minutos.")]
    [Display(Name = "Duración (minutos)")]
    public int Duracion { get; set; }

    [Display(Name = "Fecha de estreno")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La fecha de estreno es obligatoria.")]
    public DateTime FechaEstreno { get; set; } = DateTime.Today;

    [Display(Name = "URL de imagen")]
    [Url(ErrorMessage = "Debe ingresar una URL válida.")]
    public string? ImagenUrl { get; set; }

    [Display(Name = "Subir imagen")]
    public IFormFile? ImagenFile { get; set; }

    [Display(Name = "Género")]
    [Required(ErrorMessage = "Seleccione un género.")]
    public int? GeneroId { get; set; }

    [Display(Name = "Director")]
    [Required(ErrorMessage = "Seleccione un director.")]
    public int? DirectorId { get; set; }

    [Display(Name = "Reparto")]
    public ICollection<int> ActorIds { get; set; } = [];

    public IEnumerable<SelectListItem> Generos { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Directores { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Actores { get; set; } = Enumerable.Empty<SelectListItem>();
}
