using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class ActoresController : Controller
{
    private readonly ApplicationDbContext _context;

    public ActoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Actores
    public async Task<IActionResult> Index()
    {
        var actores = await _context.Actores.ToListAsync();
        
        var actoresDto = actores.Select(a => new ActorDto
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Biografia = a.Biografia,
            FechaNacimiento = a.FechaNacimiento
        }).ToList();

        return View(actoresDto);
    }

    // GET: Actores/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actores
            .Include(a => a.PeliculasActor)
                .ThenInclude(pa => pa.Pelicula)
                    .ThenInclude(p => p.Genero)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (actor == null)
        {
            return NotFound();
        }

        var actorDto = new ActorDto
        {
            Id = actor.Id,
            Nombre = actor.Nombre,
            Biografia = actor.Biografia,
            FechaNacimiento = actor.FechaNacimiento
        };

        var peliculas = actor.PeliculasActor
            .Where(pa => pa.Pelicula != null)
            .Select(pa => pa.Pelicula!)
            .Distinct()
            .OrderByDescending(p => p.FechaEstreno)
            .Select(p => new PeliculaSummaryViewModel
            {
                Id = p.Id,
                Titulo = p.Titulo,
                FechaEstreno = p.FechaEstreno,
                Genero = p.Genero.Nombre
            })
            .ToList();

        var viewModel = new ActorDetailsViewModel
        {
            Actor = actorDto,
            Peliculas = peliculas
        };

        return View(viewModel);
    }
}
