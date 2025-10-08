using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Web.Controllers;

public class GenerosController : Controller
{
    private readonly ApplicationDbContext _context;

    public GenerosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Generos
    public async Task<IActionResult> Index()
    {
        var generos = await _context.Generos.ToListAsync();
        
        var generosDto = generos.Select(g => new GeneroDto
        {
            Id = g.Id,
            Nombre = g.Nombre,
            Descripcion = g.Descripcion
        }).ToList();

        return View(generosDto);
    }

    // GET: Generos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Generos
            .FirstOrDefaultAsync(m => m.Id == id);

        if (genero == null)
        {
            return NotFound();
        }

        var generoDto = new GeneroDto
        {
            Id = genero.Id,
            Nombre = genero.Nombre,
            Descripcion = genero.Descripcion
        };

        return View(generoDto);
    }
}
