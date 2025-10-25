namespace BackendPro.Core.DTOs;

public class ResenaDto
{
    public int Id { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Comentario { get; set; } = string.Empty;
    public int Calificacion { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public int PeliculaId { get; set; }
}
