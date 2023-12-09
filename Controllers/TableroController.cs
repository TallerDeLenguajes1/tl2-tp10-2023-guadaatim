using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class TableroController : Controller
{
    private ITableroRepository _tableroRepository;
    private readonly ILogger<TableroController> _logger;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository)
    {
        _logger = logger;
        _tableroRepository = tableroRepository;
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
        ListarTablerosViewModel tableros = new ListarTablerosViewModel(_tableroRepository.GetAllTableros());
        
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
                tableros = new ListarTablerosViewModel(_tableroRepository.GetTableroByUsuario(Int32.Parse(HttpContext.Session.GetString("Id")!)));
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
    public IActionResult CreateTablero(TableroViewModel tableroNuevoVM)
    {
        if(!ModelState.IsValid)
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        } else
        {
            Tablero tableroNuevo = new Tablero(tableroNuevoVM.IdUsuarioPropietario, tableroNuevoVM.Nombre, tableroNuevoVM.Descripcion);
            _tableroRepository.CreateTablero(tableroNuevo);
            return RedirectToAction("ListarTableros");
        }
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTablero)
    {
        if (isAdmin())
        {
            TableroViewModel tablero = new TableroViewModel(_tableroRepository.GetTableroById(idTablero));
            return View(tablero);
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
        
    }

    [HttpPost]
    public IActionResult UpdateTablero(ModificarTableroViewModel tableroModificadoVM)
    {
        if(!ModelState.IsValid)
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        } else 
        {
            Tablero tableroModificado = new Tablero(tableroModificadoVM.IdUsuarioPropietario, tableroModificadoVM.Nombre, tableroModificadoVM.Descripcion);
            _tableroRepository.UpdateTablero(tableroModificadoVM.Id, tableroModificado);
            return RedirectToAction("ListarTableros");
        }
    }

    [HttpGet]
    public IActionResult EliminarTablero(int idTablero)
    {
        if (isAdmin())
        {
            Tablero tablero = _tableroRepository.GetTableroById(idTablero);
            return View(tablero);
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult DeleteTablero(Tablero tablero)
    {
        _tableroRepository.DeleteTablero(tablero.Id);
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