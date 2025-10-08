namespace BackendPro.Core.DTOs;

public class DirectorDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
}
