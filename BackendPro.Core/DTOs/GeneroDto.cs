namespace BackendPro.Core.DTOs;

public class GeneroDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class CreateGeneroDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class UpdateGeneroDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
