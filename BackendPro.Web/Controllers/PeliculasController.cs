using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Web.Controllers;

public class PeliculasController : Controller
{
    private readonly ApplicationDbContext _context;

    public PeliculasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Peliculas
    public async Task<IActionResult> Index()
    {
        var peliculas = await _context.Peliculas
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .ToListAsync();

        var peliculasDto = peliculas.Select(p => new PeliculaDto
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Sinopsis = p.Sinopsis,
            Duracion = p.Duracion,
            FechaEstreno = p.FechaEstreno,
            ImagenUrl = p.ImagenUrl,
            Genero = new GeneroDto
            {
                Id = p.Genero.Id,
                Nombre = p.Genero.Nombre,
                Descripcion = p.Genero.Descripcion
            },
            Director = new DirectorDto
            {
                Id = p.Director.Id,
                Nombre = p.Director.Nombre,
                Nacionalidad = p.Director.Nacionalidad,
                FechaNacimiento = p.Director.FechaNacimiento
            },
            Actores = p.PeliculasActor.Select(pa => new ActorDto
            {
                Id = pa.Actor.Id,
                Nombre = pa.Actor.Nombre,
                Biografia = pa.Actor.Biografia,
                FechaNacimiento = pa.Actor.FechaNacimiento
            }).ToList()
        }).ToList();

        return View(peliculasDto);
    }

    // GET: Peliculas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Peliculas
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pelicula == null)
        {
            return NotFound();
        }

        var peliculaDto = new PeliculaDto
        {
            Id = pelicula.Id,
            Titulo = pelicula.Titulo,
            Sinopsis = pelicula.Sinopsis,
            Duracion = pelicula.Duracion,
            FechaEstreno = pelicula.FechaEstreno,
            ImagenUrl = pelicula.ImagenUrl,
            Genero = new GeneroDto
            {
                Id = pelicula.Genero.Id,
                Nombre = pelicula.Genero.Nombre,
                Descripcion = pelicula.Genero.Descripcion
            },
            Director = new DirectorDto
            {
                Id = pelicula.Director.Id,
                Nombre = pelicula.Director.Nombre,
                Nacionalidad = pelicula.Director.Nacionalidad,
                FechaNacimiento = pelicula.Director.FechaNacimiento
            },
            Actores = pelicula.PeliculasActor.Select(pa => new ActorDto
            {
                Id = pa.Actor.Id,
                Nombre = pa.Actor.Nombre,
                Biografia = pa.Actor.Biografia,
                FechaNacimiento = pa.Actor.FechaNacimiento
            }).ToList()
        };

        return View(peliculaDto);
    }
}
