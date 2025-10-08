using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class DirectoresController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Directores
    public async Task<IActionResult> Index()
    {
        var directores = await _context.Directores.ToListAsync().ConfigureAwait(false);

        var directoresDto = directores.Select(d => new DirectorDto
        {
            Id = d.Id,
            Nombre = d.Nombre,
            Nacionalidad = d.Nacionalidad,
            FechaNacimiento = d.FechaNacimiento,
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
            .Include(d => d.Peliculas)
                .ThenInclude(p => p.Genero)
            .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

        if (director == null)
        {
            return NotFound();
        }

        var directorDto = new DirectorDto
        {
            Id = director.Id,
            Nombre = director.Nombre,
            Nacionalidad = director.Nacionalidad,
            FechaNacimiento = director.FechaNacimiento,
        };

        var peliculas = director.Peliculas
            .OrderByDescending(p => p.FechaEstreno)
            .Select(p => new PeliculaSummaryViewModel
            {
                Id = p.Id,
                Titulo = p.Titulo,
                FechaEstreno = p.FechaEstreno,
                Genero = p.Genero.Nombre,
            })
            .ToList();

        var viewModel = new DirectorDetailsViewModel
        {
            Director = directorDto,
            Peliculas = peliculas,
        };

        return View(viewModel);
    }
}
