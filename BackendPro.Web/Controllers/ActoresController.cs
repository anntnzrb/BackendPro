using Microsoft.AspNetCore.Mvc;
using BackendPro.Core.DTOs;
using BackendPro.Core.Interfaces;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class ActoresController(IActorService actorService) : Controller
{
    private readonly IActorService _actorService = actorService;

    // GET: Actores
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var actores = await _actorService.GetAllAsync().ConfigureAwait(false);
        return View(actores);
    }

    // GET: Actores/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _actorService.GetByIdWithPeliculasAsync(id.Value).ConfigureAwait(false);
        if (actor == null)
        {
            return NotFound();
        }

        var viewModel = new ActorDetailsViewModel
        {
            Actor = actor,
            Peliculas = (actor.Peliculas ?? []).ToList().AsReadOnly(),
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

        var createDto = new CreateActorDto
        {
            Nombre = viewModel.Nombre,
            Biografia = viewModel.Biografia,
            FechaNacimiento = viewModel.FechaNacimiento,
        };

        await _actorService.CreateAsync(createDto).ConfigureAwait(false);

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

        var actor = await _actorService.GetByIdAsync(id.Value).ConfigureAwait(false);
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

        var updateDto = new UpdateActorDto
        {
            Nombre = viewModel.Nombre,
            Biografia = viewModel.Biografia,
            FechaNacimiento = viewModel.FechaNacimiento,
        };

        await _actorService.UpdateAsync(id, updateDto).ConfigureAwait(false);

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

        var actor = await _actorService.GetByIdAsync(id.Value).ConfigureAwait(false);
        if (actor == null)
        {
            return NotFound();
        }

        return View(actor);
    }

    // POST: Actores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _actorService.DeleteAsync(id).ConfigureAwait(false);
        return RedirectToAction(nameof(Index));
    }
}
