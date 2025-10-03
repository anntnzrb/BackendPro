using Microsoft.AspNetCore.Mvc;
using BackendPro.Core.DTOs;
using BackendPro.Core.Interfaces;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class DirectoresController(IDirectorService directorService, IPeliculaService peliculaService) : Controller
{
    private readonly IDirectorService _directorService = directorService;
    private readonly IPeliculaService _peliculaService = peliculaService;

    // GET: Directores
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var directores = await _directorService.GetAllAsync().ConfigureAwait(false);
        return View(directores);
    }

    // GET: Directores/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _directorService.GetByIdWithPeliculasAsync(id.Value).ConfigureAwait(false);
        if (director == null)
        {
            return NotFound();
        }

        var peliculas = await _peliculaService.GetByDirectorIdAsync(id.Value).ConfigureAwait(false);
        var peliculasVm = peliculas
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
            Director = director,
            Peliculas = peliculasVm,
        };

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

        var createDto = new CreateDirectorDto
        {
            Nombre = viewModel.Nombre,
            Nacionalidad = viewModel.Nacionalidad,
            FechaNacimiento = viewModel.FechaNacimiento,
        };

        await _directorService.CreateAsync(createDto).ConfigureAwait(false);

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

        var director = await _directorService.GetByIdAsync(id.Value).ConfigureAwait(false);
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

        var updateDto = new UpdateDirectorDto
        {
            Nombre = viewModel.Nombre,
            Nacionalidad = viewModel.Nacionalidad,
            FechaNacimiento = viewModel.FechaNacimiento,
        };

        var result = await _directorService.UpdateAsync(id, updateDto).ConfigureAwait(false);
        if (result == null)
        {
            return NotFound();
        }

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

        var director = await _directorService.GetByIdWithPeliculasAsync(id.Value).ConfigureAwait(false);
        if (director == null)
        {
            return NotFound();
        }

        var peliculas = await _peliculaService.GetByDirectorIdAsync(id.Value).ConfigureAwait(false);
        var peliculasVm = peliculas
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
            Director = director,
            Peliculas = peliculasVm,
        };

        return View(viewModel);
    }

    // POST: Directores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _directorService.DeleteAsync(id).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
        catch (Core.Exceptions.BusinessValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var director = await _directorService.GetByIdWithPeliculasAsync(id).ConfigureAwait(false);
            if (director == null)
            {
                return NotFound();
            }

            var peliculas = await _peliculaService.GetByDirectorIdAsync(id).ConfigureAwait(false);
            var peliculasVm = peliculas
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
                Director = director,
                Peliculas = peliculasVm,
            };
            return View(viewModel);
        }
    }
}
