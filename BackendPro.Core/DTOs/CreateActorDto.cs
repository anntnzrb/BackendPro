namespace BackendPro.Core.DTOs;

public class CreateActorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public DateTime FechaNacimiento { get; set; }
}
