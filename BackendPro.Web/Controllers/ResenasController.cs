using Microsoft.AspNetCore.Mvc;
using BackendPro.Core.DTOs;
using BackendPro.Core.Interfaces;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class ResenasController(IResenaService resenaService) : Controller
{
    private readonly IResenaService _resenaService = resenaService;

    // GET: Resenas/Create?peliculaId=5
    [HttpGet]
    public IActionResult Create(int peliculaId)
    {
        var viewModel = new ResenaFormViewModel
        {
            PeliculaId = peliculaId,
        };
        return View(viewModel);
    }

    // POST: Resenas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResenaFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }

        try
        {
            var createResenaDto = new CreateResenaDto
            {
                Autor = viewModel.Autor,
                Comentario = viewModel.Comentario,
                Calificacion = viewModel.Calificacion,
                PeliculaId = viewModel.PeliculaId,
            };

            await _resenaService.CreateAsync(createResenaDto).ConfigureAwait(false);
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }
        catch (ArgumentException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Ocurrió un error al crear la reseña. Inténtelo de nuevo.";
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }
    }

    // GET: Resenas/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var resena = await _resenaService.GetByIdAsync(id).ConfigureAwait(false);
        if (resena == null)
        {
            return NotFound();
        }

        var viewModel = new ResenaFormViewModel
        {
            Id = resena.Id,
            Autor = resena.Autor,
            Comentario = resena.Comentario,
            Calificacion = resena.Calificacion,
            PeliculaId = resena.PeliculaId,
        };

        return View(viewModel);
    }

    // POST: Resenas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ResenaFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }

        try
        {
            var updateResenaDto = new UpdateResenaDto
            {
                Id = viewModel.Id.GetValueOrDefault(),
                Autor = viewModel.Autor,
                Comentario = viewModel.Comentario,
                Calificacion = viewModel.Calificacion,
                PeliculaId = viewModel.PeliculaId,
            };

            await _resenaService.UpdateAsync(id, updateResenaDto).ConfigureAwait(false);
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }
        catch (ArgumentException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Ocurrió un error al actualizar la reseña. Inténtelo de nuevo.";
            return RedirectToAction("Details", "Peliculas", new { id = viewModel.PeliculaId });
        }
    }

    // POST: Resenas/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int? peliculaId)
    {
        try
        {
            await _resenaService.DeleteAsync(id).ConfigureAwait(false);
            return RedirectToAction("Details", "Peliculas", new { id = peliculaId ?? 0 });
        }
        catch (Exception)
        {
            return RedirectToAction("Details", "Peliculas", new { id = peliculaId ?? 0 });
        }
    }

    // GET: Resenas/GetByPeliculaId/5 (for AJAX calls)
    [HttpGet]
    public async Task<IActionResult> GetByPeliculaId(int peliculaId)
    {
        var resenas = await _resenaService.GetByPeliculaIdAsync(peliculaId).ConfigureAwait(false);
        return PartialView("_ResenaList", resenas);
    }
}
