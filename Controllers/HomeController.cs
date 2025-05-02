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

    [HttpGet]
    public IActionResult AsignarAdoptante()
    {
        var mascotasDisponibles = _context.Mascotas
            .Where(m => m.EstadoAdopcion == "Disponible")
            .ToList();

        ViewBag.MascotasDisponibles = mascotasDisponibles;
        return View();
    }

    [HttpPost]
    public IActionResult AsignarAdoptante(int mascotaId, string nombreAdoptante, string contactoAdoptante)
    {
        var mascota = _context.Mascotas.FirstOrDefault(m => m.Id == mascotaId);
        if (mascota == null || mascota.EstadoAdopcion == "Adoptada")
        {
            return BadRequest("La mascota no está disponible para adopción.");
        }

        var adoptante = _context.Adoptantes.FirstOrDefault(a => a.Nombre == nombreAdoptante && a.Contacto == contactoAdoptante);
        if (adoptante == null)
        {
            adoptante = new Adoptante { Nombre = nombreAdoptante, Contacto = contactoAdoptante };
            _context.Adoptantes.Add(adoptante);
            _context.SaveChanges();
        }

        var adopcion = new Adopcion
        {
            MascotaId = mascotaId,
            Mascota = mascota,
            AdoptanteId = adoptante.Id,
            Adoptante = adoptante
        };
        _context.Adopciones.Add(adopcion);

        mascota.EstadoAdopcion = "Adoptada";
        _context.SaveChanges();

        return RedirectToAction("ListarAdopciones");
    }

    [HttpGet]
    public IActionResult ListarAdopciones()
    {
        var adopciones = _context.Mascotas
            .Select(m => new ListarAdopcionesViewModel
            {
                Mascota = m.Nombre,
                Adoptante = m.Adopcion != null ? m.Adopcion.Adoptante.Nombre : "Sin adoptante",
                EstadoAdopcion = m.EstadoAdopcion
            })
            .ToList();

        return View(adopciones);
    }
}
