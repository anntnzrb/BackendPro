using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using BackendPro.Core.DTOs;
using BackendPro.Core.Interfaces;
using BackendPro.Web.Models;

namespace BackendPro.Web.Controllers;

public class PeliculasController(
    IPeliculaService peliculaService,
    IGeneroService generoService,
    IDirectorService directorService,
    IActorService actorService,
    IFileStorageService fileStorageService) : Controller
{
    private readonly IPeliculaService _peliculaService = peliculaService;
    private readonly IGeneroService _generoService = generoService;
    private readonly IDirectorService _directorService = directorService;
    private readonly IActorService _actorService = actorService;
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private const long MaxFileSize = 5 * 1024 * 1024;

    // GET: Peliculas
    [HttpGet]
    public async Task<IActionResult> Index(int? generoId, string? searchTerm)
    {
        IEnumerable<PeliculaDto> peliculas;

        if (generoId.HasValue || !string.IsNullOrWhiteSpace(searchTerm))
        {
            peliculas = await _peliculaService.GetFilteredAsync(generoId, searchTerm).ConfigureAwait(false);
            ViewBag.GeneroId = generoId;
            ViewBag.SearchTerm = searchTerm;
        }
        else
        {
            peliculas = await _peliculaService.GetAllAsync().ConfigureAwait(false);
        }

        ViewBag.Generos = await _generoService.GetAllAsync().ConfigureAwait(false);
        return View(peliculas);
    }

    // GET: Peliculas/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _peliculaService.GetByIdAsync(id.Value).ConfigureAwait(false);
        if (pelicula == null)
        {
            return NotFound();
        }

        return View(pelicula);
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
        if (viewModel.ImagenFile != null)
        {
            if (!ValidateImageFile(viewModel.ImagenFile))
            {
                ModelState.AddModelError("ImagenFile", "Archivo no válido. Solo se permiten imágenes JPG, PNG o WEBP de hasta 5MB.");
                await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);
                return View(viewModel);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.ImagenFile.FileName).ToUpperInvariant()}";
            var tempPath = Path.GetTempFileName();

            try
            {
                await using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await viewModel.ImagenFile.CopyToAsync(stream).ConfigureAwait(false);
                }

                viewModel.ImagenUrl = await _fileStorageService.SaveFileAsync(tempPath, "peliculas", fileName).ConfigureAwait(false);
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);
            return View(viewModel);
        }

        var createDto = new CreatePeliculaDto
        {
            Titulo = viewModel.Titulo,
            Sinopsis = viewModel.Sinopsis,
            Duracion = viewModel.Duracion,
            FechaEstreno = viewModel.FechaEstreno,
            ImagenUrl = viewModel.ImagenUrl,
            GeneroId = viewModel.GeneroId!.Value,
            DirectorId = viewModel.DirectorId!.Value,
            ActorIds = viewModel.ActorIds,
        };

        await _peliculaService.CreateAsync(createDto).ConfigureAwait(false);

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

        var pelicula = await _peliculaService.GetByIdAsync(id.Value).ConfigureAwait(false);
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
            GeneroId = pelicula.Genero.Id,
            DirectorId = pelicula.Director.Id,
            ActorIds = pelicula.Actores.Select(a => a.Id).ToList(),
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

        if (viewModel.ImagenFile != null)
        {
            if (!ValidateImageFile(viewModel.ImagenFile))
            {
                ModelState.AddModelError("ImagenFile", "Archivo no válido. Solo se permiten imágenes JPG, PNG o WEBP de hasta 5MB.");
                await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);
                return View(viewModel);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.ImagenFile.FileName).ToUpperInvariant()}";
            var tempPath = Path.GetTempFileName();

            try
            {
                await using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await viewModel.ImagenFile.CopyToAsync(stream).ConfigureAwait(false);
                }

                viewModel.ImagenUrl = await _fileStorageService.SaveFileAsync(tempPath, "peliculas", fileName).ConfigureAwait(false);
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(viewModel).ConfigureAwait(false);
            return View(viewModel);
        }

        var updateDto = new UpdatePeliculaDto
        {
            Titulo = viewModel.Titulo,
            Sinopsis = viewModel.Sinopsis,
            Duracion = viewModel.Duracion,
            FechaEstreno = viewModel.FechaEstreno,
            ImagenUrl = viewModel.ImagenUrl,
            GeneroId = viewModel.GeneroId!.Value,
            DirectorId = viewModel.DirectorId!.Value,
            ActorIds = viewModel.ActorIds,
        };

        await _peliculaService.UpdateAsync(id, updateDto).ConfigureAwait(false);

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

        var pelicula = await _peliculaService.GetByIdAsync(id.Value).ConfigureAwait(false);
        if (pelicula == null)
        {
            return NotFound();
        }

        return View(pelicula);
    }

    // POST: Peliculas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _peliculaService.DeleteAsync(id).ConfigureAwait(false);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateSelectListsAsync(PeliculaFormViewModel viewModel)
    {
        var generos = await _generoService.GetAllAsync().ConfigureAwait(false);
        var directores = await _directorService.GetAllAsync().ConfigureAwait(false);
        var actores = await _actorService.GetAllAsync().ConfigureAwait(false);

        viewModel.Generos = generos
            .OrderBy(g => g.Nombre, StringComparer.Ordinal)
            .Select(g => new SelectListItem
            {
                Value = g.Id.ToString(CultureInfo.InvariantCulture),
                Text = g.Nombre,
                Selected = viewModel.GeneroId == g.Id,
            })
            .ToList();

        viewModel.Directores = directores
            .OrderBy(d => d.Nombre, StringComparer.Ordinal)
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
            .OrderBy(a => a.Nombre, StringComparer.Ordinal)
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(CultureInfo.InvariantCulture),
                Text = a.Nombre,
                Selected = actorIds.Contains(a.Id),
            })
            .ToList();
    }

    private static bool ValidateImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }

        if (file.Length > MaxFileSize)
        {
            return false;
        }

        var extension = Path.GetExtension(file.FileName).ToUpperInvariant();
        var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
        return allowedExtensions.Contains(extension);
    }
}
