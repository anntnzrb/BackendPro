using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Core.DTOs;
using BackendPro.Core.Entities;
using BackendPro.Infrastructure.Data;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class GenerosController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Generos
    [HttpGet]
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
    [HttpGet]
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

    // GET: Generos/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new GeneroFormViewModel());
    }

    // POST: Generos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GeneroFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var genero = new Genero
        {
            Nombre = viewModel.Nombre,
            Descripcion = viewModel.Descripcion,
        };

        _context.Generos.Add(genero);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Generos/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Generos
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id).ConfigureAwait(false);

        if (genero == null)
        {
            return NotFound();
        }

        var viewModel = new GeneroFormViewModel
        {
            Id = genero.Id,
            Nombre = genero.Nombre,
            Descripcion = genero.Descripcion,
        };

        return View(viewModel);
    }

    // POST: Generos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GeneroFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var genero = await _context.Generos
            .FirstOrDefaultAsync(g => g.Id == id).ConfigureAwait(false);

        if (genero == null)
        {
            return NotFound();
        }

        genero.Nombre = viewModel.Nombre;
        genero.Descripcion = viewModel.Descripcion;

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    // GET: Generos/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Generos
            .Include(g => g.Peliculas)
                .ThenInclude(p => p.Director)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id).ConfigureAwait(false);

        if (genero == null)
        {
            return NotFound();
        }

        var viewModel = MapGeneroDetails(genero);

        return View(viewModel);
    }

    // POST: Generos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var genero = await _context.Generos
            .Include(g => g.Peliculas)
                .ThenInclude(p => p.Director)
            .FirstOrDefaultAsync(g => g.Id == id).ConfigureAwait(false);

        if (genero == null)
        {
            return NotFound();
        }

        if (genero.Peliculas.Count != 0)
        {
            ModelState.AddModelError(string.Empty, "No se puede eliminar el género porque tiene películas asociadas.");
            var viewModel = MapGeneroDetails(genero);
            return View(viewModel);
        }

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    private static GeneroDetailsViewModel MapGeneroDetails(Genero genero)
    {
        var dto = new GeneroDto
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

        return new GeneroDetailsViewModel
        {
            Genero = dto,
            Peliculas = peliculas,
        };
    }
}
