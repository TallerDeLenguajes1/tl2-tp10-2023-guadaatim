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
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult AltaTablero()
    {
        try
        {
            if (isAdmin())
            {
                return View(new CrearTableroViewModel());
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpPost]
    public IActionResult CreateTablero(CrearTableroViewModel tableroNuevoVM)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTablero)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpPost]
    public IActionResult UpdateTablero(ModificarTableroViewModel tableroModificadoVM)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }      
    }

    [HttpGet]
    public IActionResult EliminarTablero(int idTablero)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpPost]
    public IActionResult DeleteTablero(Tablero tablero)
    {
        try
        {
            _tableroRepository.DeleteTablero(tablero.Id);
            return RedirectToAction("ListarTableros");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
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