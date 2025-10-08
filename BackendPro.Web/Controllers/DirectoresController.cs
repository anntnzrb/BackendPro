using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class DirectoresController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Directores
    [HttpGet]
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
    [HttpGet]
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

        var viewModel = MapDirectorDetails(director);

        return View(viewModel);
    }

    // GET: Directores/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new DirectorFormViewModel
        {
            FechaNacimiento = DateTime.Today,
        });
    }

    // POST: Directores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DirectorFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var director = new Director
        {
            Nombre = viewModel.Nombre,
            Nacionalidad = viewModel.Nacionalidad,
            FechaNacimiento = viewModel.FechaNacimiento,
        };

        _context.Directores.Add(director);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Directores/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directores
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id).ConfigureAwait(false);

        if (director == null)
        {
            return NotFound();
        }

        var viewModel = new DirectorFormViewModel
        {
            Id = director.Id,
            Nombre = director.Nombre,
            Nacionalidad = director.Nacionalidad,
            FechaNacimiento = director.FechaNacimiento,
        };

        return View(viewModel);
    }

    // POST: Directores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DirectorFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var director = await _context.Directores
            .FirstOrDefaultAsync(d => d.Id == id).ConfigureAwait(false);

        if (director == null)
        {
            return NotFound();
        }

        director.Nombre = viewModel.Nombre;
        director.Nacionalidad = viewModel.Nacionalidad;
        director.FechaNacimiento = viewModel.FechaNacimiento;

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Directores/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Directores
            .Include(d => d.Peliculas)
                .ThenInclude(p => p.Genero)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id).ConfigureAwait(false);

        if (director == null)
        {
            return NotFound();
        }

        var viewModel = MapDirectorDetails(director);

        return View(viewModel);
    }

    // POST: Directores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var director = await _context.Directores
            .Include(d => d.Peliculas)
                .ThenInclude(p => p.Genero)
            .FirstOrDefaultAsync(d => d.Id == id).ConfigureAwait(false);

        if (director == null)
        {
            return NotFound();
        }

        if (director.Peliculas.Count != 0)
        {
            ModelState.AddModelError(string.Empty, "No se puede eliminar el director porque tiene películas asociadas.");
            var viewModel = MapDirectorDetails(director);
            return View(viewModel);
        }

        _context.Directores.Remove(director);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    private static DirectorDetailsViewModel MapDirectorDetails(Director director)
    {
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

        return new DirectorDetailsViewModel
        {
            Director = directorDto,
            Peliculas = peliculas,
        };
    }
}
