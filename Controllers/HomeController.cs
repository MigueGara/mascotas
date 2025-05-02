using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaAdopcionMascotas.Models;

namespace SistemaAdopcionMascotas.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class MascotasController : Controller
{
    private readonly ApplicationDbContext _context;

    public MascotasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Mascota mascota)
    {
        if (ModelState.IsValid)
        {
            _context.Mascotas.Add(mascota);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(mascota);
    }
}
