using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Web.Controllers;

public class DirectoresController : Controller
{
    private readonly ApplicationDbContext _context;

    public DirectoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Directores
    public async Task<IActionResult> Index()
    {
        var directores = await _context.Directores.ToListAsync();
        
        var directoresDto = directores.Select(d => new DirectorDto
        {
            Id = d.Id,
            Nombre = d.Nombre,
            Nacionalidad = d.Nacionalidad,
            FechaNacimiento = d.FechaNacimiento
        }).ToList();

        return View(directoresDto);
    }

    // GET: Directores/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directores
            .FirstOrDefaultAsync(m => m.Id == id);

        if (director == null)
        {
            return NotFound();
        }

        var directorDto = new DirectorDto
        {
            Id = director.Id,
            Nombre = director.Nombre,
            Nacionalidad = director.Nacionalidad,
            FechaNacimiento = director.FechaNacimiento
        };

        return View(directorDto);
    }
}
