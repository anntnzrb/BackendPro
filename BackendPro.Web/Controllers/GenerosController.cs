using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class GenerosController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Generos
    public async Task<IActionResult> Index()
    {
        var generos = await _context.Generos.ToListAsync().ConfigureAwait(false);

        var generosDto = generos.Select(g => new GeneroDto
        {
            Id = g.Id,
            Nombre = g.Nombre,
            Descripcion = g.Descripcion,
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
            .Include(g => g.Peliculas)
                .ThenInclude(p => p.Director)
            .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

        if (genero == null)
        {
            return NotFound();
        }

        var generoDto = new GeneroDto
        {
            Id = genero.Id,
            Nombre = genero.Nombre,
            Descripcion = genero.Descripcion,
        };

        var peliculas = genero.Peliculas
            .OrderByDescending(p => p.FechaEstreno)
            .Select(p => new PeliculaSummaryViewModel
            {
                Id = p.Id,
                Titulo = p.Titulo,
                FechaEstreno = p.FechaEstreno,
                Genero = genero.Nombre,
            })
            .ToList();

        var viewModel = new GeneroDetailsViewModel
        {
            Genero = generoDto,
            Peliculas = peliculas,
        };

        return View(viewModel);
    }
}
