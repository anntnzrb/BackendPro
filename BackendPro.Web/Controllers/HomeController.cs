using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Web.Models;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Web.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var stats = new HomeViewModel
        {
            TotalPeliculas = await _context.Peliculas.CountAsync().ConfigureAwait(false),
            TotalGeneros = await _context.Generos.CountAsync().ConfigureAwait(false),
            TotalDirectores = await _context.Directores.CountAsync().ConfigureAwait(false),
            TotalActores = await _context.Actores.CountAsync().ConfigureAwait(false),
        };

        return View(stats);
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
