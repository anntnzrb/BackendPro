using System.ComponentModel.DataAnnotations;

namespace BackendPro.Web.Models;

public class ActorFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Biografía")]
    [StringLength(1000, ErrorMessage = "La biografía no puede superar los 1000 caracteres.")]
    public string? Biografia { get; set; }

    [Display(Name = "Fecha de nacimiento")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;
}
