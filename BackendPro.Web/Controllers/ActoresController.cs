using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class ActoresController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Actores
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var actores = await _context.Actores.ToListAsync().ConfigureAwait(false);

        var actoresDto = actores.Select(a => new ActorDto
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Biografia = a.Biografia,
            FechaNacimiento = a.FechaNacimiento,
        }).ToList();

        return View(actoresDto);
    }

    // GET: Actores/Details/5
    [HttpGet]
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
            .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

        if (actor == null)
        {
            return NotFound();
        }

        var actorDto = new ActorDto
        {
            Id = actor.Id,
            Nombre = actor.Nombre,
            Biografia = actor.Biografia,
            FechaNacimiento = actor.FechaNacimiento,
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
                Genero = p.Genero.Nombre,
            })
            .ToList();

        var viewModel = new ActorDetailsViewModel
        {
            Actor = actorDto,
            Peliculas = peliculas,
        };

        return View(viewModel);
    }

    // GET: Actores/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new ActorFormViewModel
        {
            FechaNacimiento = DateTime.Today,
        });
    }

    // POST: Actores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ActorFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var actor = new Actor
        {
            Nombre = viewModel.Nombre,
            Biografia = viewModel.Biografia,
            FechaNacimiento = viewModel.FechaNacimiento,
        };

        _context.Actores.Add(actor);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Actores/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actores
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

        if (actor == null)
        {
            return NotFound();
        }

        var viewModel = new ActorFormViewModel
        {
            Id = actor.Id,
            Nombre = actor.Nombre,
            Biografia = actor.Biografia,
            FechaNacimiento = actor.FechaNacimiento,
        };

        return View(viewModel);
    }

    // POST: Actores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ActorFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var actor = await _context.Actores
            .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

        if (actor == null)
        {
            return NotFound();
        }

        actor.Nombre = viewModel.Nombre;
        actor.Biografia = viewModel.Biografia;
        actor.FechaNacimiento = viewModel.FechaNacimiento;

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Actores/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actores
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

        if (actor == null)
        {
            return NotFound();
        }

        var viewModel = new ActorDto
        {
            Id = actor.Id,
            Nombre = actor.Nombre,
            Biografia = actor.Biografia,
            FechaNacimiento = actor.FechaNacimiento,
        };

        return View(viewModel);
    }

    // POST: Actores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var actor = await _context.Actores
            .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

        if (actor != null)
        {
            _context.Actores.Remove(actor);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        return RedirectToAction(nameof(Index));
    }
}
