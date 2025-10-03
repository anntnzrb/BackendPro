namespace BackendPro.Core.DTOs;

public class UpdateActorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public DateTime FechaNacimiento { get; set; }
}
