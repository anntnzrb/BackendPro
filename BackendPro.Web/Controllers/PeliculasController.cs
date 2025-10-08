using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class PeliculasController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Peliculas
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var peliculas = await _context.Peliculas
            .Include(p => p.Genero)
            .Include(p => p.Director)
            .Include(p => p.PeliculasActor)
                .ThenInclude(pa => pa.Actor)
            .ToListAsync().ConfigureAwait(false);

        var peliculasDto = peliculas.Select(MapToDto).ToList();

        return View(peliculasDto);
    }

    // GET: Peliculas/Details/5
    [HttpGet]
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
            .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

        if (pelicula == null)
        {
            return NotFound();
        }

        var peliculaDto = MapToDto(pelicula);

        return View(peliculaDto);
    }

    // GET: Peliculas/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new PeliculaFormViewModel
        {
            FechaEstreno = DateTime.Today,
            Duracion = 120,
            ActorIds = [],
        };

        await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);

        return View(viewModel);
    }

    // POST: Peliculas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PeliculaFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);
            return View(viewModel);
        }

        var pelicula = new Pelicula
        {
            Titulo = viewModel.Titulo,
            Sinopsis = viewModel.Sinopsis,
            Duracion = viewModel.Duracion,
            FechaEstreno = viewModel.FechaEstreno,
            ImagenUrl = string.IsNullOrWhiteSpace(viewModel.ImagenUrl) ? null : viewModel.ImagenUrl,
            GeneroId = viewModel.GeneroId!.Value,
            DirectorId = viewModel.DirectorId!.Value,
            PeliculasActor = viewModel.ActorIds
                .Distinct()
                .Select(actorId => new PeliculaActor { ActorId = actorId })
                .ToList(),
        };

        _context.Peliculas.Add(pelicula);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Peliculas/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Peliculas
            .Include(p => p.PeliculasActor)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

        if (pelicula == null)
        {
            return NotFound();
        }

        var viewModel = new PeliculaFormViewModel
        {
            Id = pelicula.Id,
            Titulo = pelicula.Titulo,
            Sinopsis = pelicula.Sinopsis,
            Duracion = pelicula.Duracion,
            FechaEstreno = pelicula.FechaEstreno,
            ImagenUrl = pelicula.ImagenUrl,
            GeneroId = pelicula.GeneroId,
            DirectorId = pelicula.DirectorId,
            ActorIds = pelicula.PeliculasActor.Select(pa => pa.ActorId).ToList(),
        };

        await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);

        return View(viewModel);
    }

    // POST: Peliculas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PeliculaFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);
            return View(viewModel);
        }

        var pelicula = await _context.Peliculas
            .Include(p => p.PeliculasActor)
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

        if (pelicula == null)
        {
            return NotFound();
        }

        pelicula.Titulo = viewModel.Titulo;
        pelicula.Sinopsis = viewModel.Sinopsis;
        pelicula.Duracion = viewModel.Duracion;
        pelicula.FechaEstreno = viewModel.FechaEstreno;
        pelicula.ImagenUrl = string.IsNullOrWhiteSpace(viewModel.ImagenUrl) ? null : viewModel.ImagenUrl;
        pelicula.GeneroId = viewModel.GeneroId!.Value;
        pelicula.DirectorId = viewModel.DirectorId!.Value;

        var selectedActorIds = viewModel.ActorIds.Distinct().ToList();
        var currentActorIds = pelicula.PeliculasActor.Select(pa => pa.ActorId).ToList();

        var actorsToRemove = pelicula.PeliculasActor.Where(pa => !selectedActorIds.Contains(pa.ActorId)).ToList();
        foreach (var peliculaActor in actorsToRemove)
        {
            pelicula.PeliculasActor.Remove(peliculaActor);
        }

        foreach (var actorId in selectedActorIds)
        {
            if (!currentActorIds.Contains(actorId))
            {
                pelicula.PeliculasActor.Add(new PeliculaActor { ActorId = actorId, PeliculaId = pelicula.Id });
            }
        }

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Peliculas/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
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
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

        if (pelicula == null)
        {
            return NotFound();
        }

        var peliculaDto = MapToDto(pelicula);

        return View(peliculaDto);
    }

    // POST: Peliculas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pelicula = await _context.Peliculas
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

        if (pelicula == null)
        {
            return NotFound();
        }

        _context.Peliculas.Remove(pelicula);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateSelectListsAsync(PeliculaFormViewModel viewModel)
    {
        var generos = await _context.Generos
            .AsNoTracking()
            .OrderBy(g => g.Nombre)
            .ToListAsync().ConfigureAwait(false);

        var directores = await _context.Directores
            .AsNoTracking()
            .OrderBy(d => d.Nombre)
            .ToListAsync().ConfigureAwait(false);

        var actores = await _context.Actores
            .AsNoTracking()
            .OrderBy(a => a.Nombre)
            .ToListAsync().ConfigureAwait(false);

        viewModel.Generos = generos
            .Select(g => new SelectListItem
            {
                Value = g.Id.ToString(CultureInfo.InvariantCulture),
                Text = g.Nombre,
                Selected = viewModel.GeneroId == g.Id,
            })
            .ToList();

        viewModel.Directores = directores
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(CultureInfo.InvariantCulture),
                Text = d.Nombre,
                Selected = viewModel.DirectorId == d.Id,
            })
            .ToList();

        var actorIds = viewModel.ActorIds;
        if (actorIds == null)
        {
            actorIds = [];
            viewModel.ActorIds = actorIds;
        }

        viewModel.Actores = actores
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(CultureInfo.InvariantCulture),
                Text = a.Nombre,
                Selected = actorIds.Contains(a.Id),
            })
            .ToList();
    }

    private static PeliculaDto MapToDto(Pelicula pelicula)
    {
        return new PeliculaDto
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
                Descripcion = pelicula.Genero.Descripcion,
            },
            Director = new DirectorDto
            {
                Id = pelicula.Director.Id,
                Nombre = pelicula.Director.Nombre,
                Nacionalidad = pelicula.Director.Nacionalidad,
                FechaNacimiento = pelicula.Director.FechaNacimiento,
            },
            Actores = pelicula.PeliculasActor
                .Where(pa => pa.Actor != null)
                .Select(pa => new ActorDto
                {
                    Id = pa.Actor.Id,
                    Nombre = pa.Actor.Nombre,
                    Biografia = pa.Actor.Biografia,
                    FechaNacimiento = pa.Actor.FechaNacimiento,
                })
                .ToList(),
        };
    }
}
