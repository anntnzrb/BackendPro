using Microsoft.AspNetCore.Mvc;
using BackendPro.Core.DTOs;
using BackendPro.Core.Interfaces;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class GenerosController(IGeneroService generoService) : Controller
{
    private readonly IGeneroService _generoService = generoService;

    // GET: Generos
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var generos = await _generoService.GetAllAsync().ConfigureAwait(false);
        return View(generos);
    }

    // GET: Generos/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _generoService.GetByIdWithPeliculasAsync(id.Value).ConfigureAwait(false);
        if (genero == null)
        {
            return NotFound();
        }

        var viewModel = new GeneroDetailsViewModel
        {
            Genero = genero,
            Peliculas = [],
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

        var createDto = new CreateGeneroDto
        {
            Nombre = viewModel.Nombre,
            Descripcion = viewModel.Descripcion,
        };

        await _generoService.CreateAsync(createDto).ConfigureAwait(false);

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

        var genero = await _generoService.GetByIdAsync(id.Value).ConfigureAwait(false);
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

        var updateDto = new UpdateGeneroDto
        {
            Nombre = viewModel.Nombre,
            Descripcion = viewModel.Descripcion,
        };

        await _generoService.UpdateAsync(id, updateDto).ConfigureAwait(false);

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

        var genero = await _generoService.GetByIdWithPeliculasAsync(id.Value).ConfigureAwait(false);
        if (genero == null)
        {
            return NotFound();
        }

        var viewModel = new GeneroDetailsViewModel
        {
            Genero = genero,
            Peliculas = [],
        };

        return View(viewModel);
    }

    // POST: Generos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _generoService.DeleteAsync(id).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
        catch (Core.Exceptions.BusinessValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var genero = await _generoService.GetByIdWithPeliculasAsync(id).ConfigureAwait(false);
            if (genero == null)
            {
                return NotFound();
            }

            var viewModel = new GeneroDetailsViewModel
            {
                Genero = genero,
                Peliculas = [],
            };
            return View(viewModel);
        }
    }
}
