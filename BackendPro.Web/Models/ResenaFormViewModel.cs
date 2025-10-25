using System.ComponentModel.DataAnnotations;

namespace BackendPro.Web.Models;

public class ResenaFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "El autor es obligatorio.")]
    [StringLength(100, ErrorMessage = "El autor no puede superar los 100 caracteres.")]
    [Display(Name = "Autor")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "El comentario es obligatorio.")]
    [StringLength(1000, ErrorMessage = "El comentario no puede superar los 1000 caracteres.")]
    [Display(Name = "Comentario")]
    public string Comentario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La calificación es obligatoria.")]
    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
    [Display(Name = "Calificación")]
    public int Calificacion { get; set; } = 5;

    [Required]
    public int PeliculaId { get; set; }

    // Additional computed properties
    public string StarDisplay => new string('★', Calificacion) + new string('☆', 5 - Calificacion);
}
