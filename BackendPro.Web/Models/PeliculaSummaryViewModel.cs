namespace BackendPro.Web.Models;

public class PeliculaSummaryViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaEstreno { get; set; }
    public string Genero { get; set; } = string.Empty;
}
