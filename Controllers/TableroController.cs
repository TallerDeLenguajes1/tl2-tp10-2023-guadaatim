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

    [HttpGet]
    public IActionResult ListarTableros()
    {
        try
        {
            if(isAdmin())
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<TableroViewModel> tablerosVM = _tableroRepository.GetTablerosViewModel();
                List<TableroViewModel> tablerosPropiosVM = _tableroRepository.GetTablerosViewModelByUsuario(id);
                ListarTablerosViewModel tableros = new ListarTablerosViewModel(tablerosVM, tablerosPropiosVM);
                return View(tableros);
            } else
            {
                if (isOperador())
                {
                    return RedirectToAction("ListarTablerosOperador");
                } else
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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
    public IActionResult ListarTablerosOperador()
    {
        try
        {
            if (isOperador())
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<TableroViewModel> tablerosVM = _tableroRepository.GetTablerosViewModelByTarea(id);
                List<TableroViewModel> tablerosPropiosVM = _tableroRepository.GetTablerosViewModelByUsuario(id);
                ListarTablerosViewModel tableros = new ListarTablerosViewModel(tablerosVM, tablerosPropiosVM);
                return View(tableros);
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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
            if (isAdmin() || isOperador())
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
            if (isAdmin() || isOperador())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else
                {
                    Tablero tableroNuevo = new Tablero(tableroNuevoVM);
                    tableroNuevo.IdUsuarioPropietario = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                    _tableroRepository.CreateTablero(tableroNuevo);
                    return RedirectToAction("ListarTableros");
                }
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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
            if (isAdmin() || isOperador())
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
            if (isAdmin() || isOperador())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else 
                {
                    Tablero tableroModificado = new Tablero(tableroModificadoVM);
                    _tableroRepository.UpdateTablero(tableroModificado);
                    return RedirectToAction("ListarTableros");
                }
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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
            if (isAdmin() || isOperador())
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
    public IActionResult DeleteTablero(TableroViewModel tablero)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                _tableroRepository.DeleteTablero(tablero.Id);
                return RedirectToAction("ListarTableros");
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR    
            }
            
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

    private bool isOperador()
    {
        if(HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Operador")
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