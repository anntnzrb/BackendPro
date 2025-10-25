using System.ComponentModel.DataAnnotations;

namespace BackendPro.Core.DTOs;

public class UpdateResenaDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El autor es requerido")]
    [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "El comentario es requerido")]
    [StringLength(1000, ErrorMessage = "El comentario no puede exceder 1000 caracteres")]
    public string Comentario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La calificación es requerida")]
    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
    public int Calificacion { get; set; }

    [Required]
    public int PeliculaId { get; set; }
}
