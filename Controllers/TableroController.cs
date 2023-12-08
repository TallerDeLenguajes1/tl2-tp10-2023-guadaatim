using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class TableroController : Controller
{
    private ITableroRepository tableroRepository;
    private readonly ILogger<TableroController> _logger;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepository = new TableroRepository();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ListarTableros()
    {
        ListarTablerosViewModel tableros = new ListarTablerosViewModel(tableroRepository.GetAllTableros());
        
        if(isAdmin())
        {
            if(tableros != null)
            {
                return View(tableros);
            } else
            {
                return NotFound();
            }
        } else
        {
            if (HttpContext.Session.GetString("Rol") == "Operador")
            {
                tableros = new ListarTablerosViewModel(tableroRepository.GetTableroByUsuario(Int32.Parse(HttpContext.Session.GetString("Id")!)));
                return View(tableros);
            } else
            {
                return NotFound();
            }
        }
    }

    [HttpGet]
    public IActionResult AltaTablero()
    {
        if (isAdmin())
        {
            return View(new TableroViewModel());
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult CreateTablero(Tablero tableroNuevo)
    {
        tableroRepository.CreateTablero(tableroNuevo);
        return RedirectToAction("ListarTableros");
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTablero)
    {
        if (isAdmin())
        {
            TableroViewModel tablero = new TableroViewModel(tableroRepository.GetTableroById(idTablero));
            return View(tablero);
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
        
    }

    [HttpPost]
    public IActionResult UpdateTablero(Tablero tableroModificado)
    {
        tableroRepository.UpdateTablero(tableroModificado.Id, tableroModificado);
        return RedirectToAction("ListarTableros");
    }

    [HttpGet]
    public IActionResult EliminarTablero(int idTablero)
    {
        if (isAdmin())
        {
            Tablero tablero = tableroRepository.GetTableroById(idTablero);
            return View(tablero);
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult DeleteTablero(Tablero tablero)
    {
        tableroRepository.DeleteTablero(tablero.Id);
        return RedirectToAction("ListarTableros");
    }

    private bool isAdmin()
    {
        if (HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Administrador")
        {
            return true;
        } else
        {
            return false;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}