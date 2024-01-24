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
    private IUsuarioRepository _usuarioRepository;
    private readonly ILogger<TableroController> _logger;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _tableroRepository = tableroRepository;
        _usuarioRepository = usuarioRepository;
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
            if(isAdmin())
            {
                //dice q esta nulo ????
                string nombre = HttpContext.Session.GetString("NombreDeUsuario");
                string rol = HttpContext.Session.GetString("Rol");
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                ListarTablerosViewModel tableros = new ListarTablerosViewModel(nombre!, rol!, _tableroRepository.GetAllTableros(), usuarios);
                int i = 0;

                if(tableros != null)
                {
                    return View(tableros);
                } else
                {
                    return NotFound();
                }
            } else
            {
                if (isOperador())
                {
                    ListarTablerosViewModel tableros = new ListarTablerosViewModel(HttpContext.Session.GetString("NombreDeUsuario")!, HttpContext.Session.GetString("Rol")!, _tableroRepository.GetTableroByUsuario(Int32.Parse(HttpContext.Session.GetString("Id")!)), _usuarioRepository.GetAllUsuarios());
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
            if (isAdmin())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else
                {
                    Tablero tableroNuevo = new Tablero(Convert.ToInt32(HttpContext.Session.GetString("Id")), tableroNuevoVM.Nombre, tableroNuevoVM.Descripcion);
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
            if (isAdmin())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else 
                {
                    Tablero tableroModificado = new Tablero(tableroModificadoVM.Id, tableroModificadoVM.IdUsuarioPropietario, tableroModificadoVM.Nombre, tableroModificadoVM.Descripcion);
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