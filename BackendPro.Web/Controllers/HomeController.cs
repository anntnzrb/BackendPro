using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendPro.Web.Models;
using BackendPro.Infrastructure.Data;

namespace BackendPro.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var stats = new HomeViewModel
        {
            TotalPeliculas = await _context.Peliculas.CountAsync(),
            TotalGeneros = await _context.Generos.CountAsync(),
            TotalDirectores = await _context.Directores.CountAsync(),
            TotalActores = await _context.Actores.CountAsync()
        };

        return View(stats);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
