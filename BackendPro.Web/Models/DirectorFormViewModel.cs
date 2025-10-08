using System.ComponentModel.DataAnnotations;

namespace BackendPro.Web.Models;

public class DirectorFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [StringLength(50, ErrorMessage = "La nacionalidad no puede superar los 50 caracteres.")]
    public string Nacionalidad { get; set; } = string.Empty;

    [Display(Name = "Fecha de nacimiento")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;
}
