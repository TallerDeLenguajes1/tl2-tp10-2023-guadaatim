using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository _tableroRepository;
    private IUsuarioRepository _usuarioRepository;
    private ITareaRepository _tareaRepository;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ITareaRepository tareaRepository)
    {
        _logger = logger;
        _tableroRepository = tableroRepository;
        _usuarioRepository = usuarioRepository;
        _tareaRepository = tareaRepository;
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
                List<Tablero> tablerosVM = _tableroRepository.GetAllTableros();
                List<Tablero> tablerosPropiosVM = _tableroRepository.GetTableroByUsuario(id);
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                ListarTablerosViewModel tableros = new ListarTablerosViewModel(tablerosVM, tablerosPropiosVM);
                return View(tableros);
            } else
            {
                if (isOperador())
                {
                    return RedirectToAction("ListarTablerosOperador");
                } else
                {
                    return RedirectToAction("Error");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString()); 
            return RedirectToAction("Error");
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
                List<Tablero> tablerosVM = _tableroRepository.GetTablerosByTarea(id);
                List<Tablero> tablerosPropiosVM = _tableroRepository.GetTableroByUsuario(id);
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                ListarTablerosViewModel tableros = new ListarTablerosViewModel(tablerosVM, tablerosPropiosVM);
                return View(tableros);
            } else
            {
                return RedirectToAction("Error");
            }  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString()); 
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ListarMisTableros()
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<Tablero> tableros = _tableroRepository.GetTableroByUsuario(id);
                ListarTablerosViewModel tablerosVM = new ListarTablerosViewModel(tableros);
                return View(tablerosVM);
            } else
            {
                return RedirectToAction("Error");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString()); 
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult AltaTablero()
    {
        try
        {
            if (isAdmin())
            {
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                return View(new CrearTableroViewModel(usuarios));
            } else
            {
                if (isOperador())
                {
                    return View("AltaTableroOperador", new CrearTableroViewModel());
                } else
                {
                    return RedirectToAction("Error");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
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
                    tableroNuevoVM.Error = "Error al crear el tablero.";
                    return View("AltaTablero", tableroNuevoVM);
                } else
                {
                    Tablero tableroNuevo = new Tablero(tableroNuevoVM);
                    _tableroRepository.CreateTablero(tableroNuevo);
                    return RedirectToAction("ListarTableros");
                }
            } else
            {
                if (isOperador())
                {
                    if (!ModelState.IsValid)
                    {
                        tableroNuevoVM.Error = "Error al crear el tablero.";
                        return View("AltaTablero",tableroNuevoVM);
                    } else
                    {   
                        Tablero tableroNuevo = new Tablero(tableroNuevoVM);
                        tableroNuevo.IdUsuarioPropietario = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                        _tableroRepository.CreateTablero(tableroNuevo);
                        return RedirectToAction("ListarTableros");
                    }
                } else
                {
                    return RedirectToAction("Error");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTablero)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                ModificarTableroViewModel tablero = new ModificarTableroViewModel(_tableroRepository.GetTableroById(idTablero));
                return View(tablero);
            } else
            {
                return RedirectToAction("Error");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
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
                    tableroModificadoVM.Error = "Error al modificar el tablero.";
                    return View("ModificarTablero", tableroModificadoVM);
                } else 
                {
                    Tablero tableroModificado = new Tablero(tableroModificadoVM);
                    _tableroRepository.UpdateTablero(tableroModificado);
                    return RedirectToAction("ListarTableros");
                }
            } else
            {
                return RedirectToAction("Error");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
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
                return RedirectToAction("Error");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult DeleteTablero(int idTablero)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                _tareaRepository.DeleteTareaByTablero(idTablero);
                _tableroRepository.DeleteTablero(idTablero);
                return RedirectToAction("ListarTableros");
            } else
            {
                return RedirectToAction("Error");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
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