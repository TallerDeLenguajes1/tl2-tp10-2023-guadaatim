using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class TareaController : Controller
{
    private ITareaRepository _tareaRepository;
    private IUsuarioRepository _usuarioRepository;
    private ITableroRepository _tableroRepository;
    private ILogger<TareaController> _logger;

    public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository)
    {
        _logger = logger;
        _tareaRepository = tareaRepository;
        _usuarioRepository = usuarioRepository;
        _tableroRepository = tableroRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ListarTareas()
    {
        try
        {
            if(isAdmin())
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<Tarea> tareas = _tareaRepository.GetAllTareas();
                List<Tarea> tareasPropias = _tareaRepository.GetAllTareasByUsuario(id);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, tareasPropias);
                return View(tareasVM);
            } else
            {
                if (isOperador())
                {
                    return RedirectToAction("ListarTareasOperador");
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
    public IActionResult ListarTareasOperador()
    {
        try
        {
            if(isOperador())
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<Tarea> tareas = _tareaRepository.GetAllTareas();
                List<Tarea> tareasPropias = _tareaRepository.GetAllTareasByUsuario(id);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, tareasPropias);
                return View(tareasVM);
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
    public IActionResult MostrarTarea(int idTablero)
    {
        try
        {
            if (isOperador() || isAdmin()) 
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                Tarea tarea = _tareaRepository.GetTareaByUsuarioAndTablero(id, idTablero);
                TareaViewModel tareaVM = new TareaViewModel(tarea);
                return View(tareaVM);
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
    public IActionResult ListarTareasPorTablero(int idTablero)
    {
        try
        {
            int idUsuario = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
            if (isAdmin() || _tableroRepository.PerteneceTablero(idUsuario, idTablero))
            {
                List<Tarea> tareas = _tareaRepository.GetAllTareasByTablero(idTablero);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas);
                return View(tareasVM);
            } else
            {
                if(isOperador())
                {
                    return RedirectToAction("ListarTareasPorTableroOperador", new {idTablero = idTablero});
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
    public IActionResult ListarTareasPorTableroOperador(int idTablero)
    {
        try
        {
            if (isOperador())
            {
                List<Tarea> tareas = _tareaRepository.GetAllTareasByTablero(idTablero);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas);
                return View(tareasVM);
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
    public IActionResult ListarMisTareas()
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<Tarea> tareas = _tareaRepository.GetAllTareasByUsuario(id);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas);
                return View(tareasVM);
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
    public IActionResult AltaTarea(int idTablero)
    {
        try
        {
            if(isAdmin() || isOperador())
            {
                ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(_usuarioRepository.GetAllUsuarios());
                return View(new CrearTareaViewModel(idTablero, usuarios));
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
    public IActionResult CreateTarea(CrearTareaViewModel tareaNuevaVM)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                if(!ModelState.IsValid)
                {
                    tareaNuevaVM.Error  ="Error al crear la tarea.";
                    return View("AltaTarea", tareaNuevaVM); 
                } else
                {
                    Tarea tareaNueva = new Tarea(tareaNuevaVM);
                    _tareaRepository.CreateTarea(tareaNuevaVM.IdTablero, tareaNueva);
                    return RedirectToAction("ListarTareas");
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
    public IActionResult ModificarTarea(int idTarea, int idTablero)
    {
        try
        {
            int idUsuario = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
            if(isAdmin() || _tableroRepository.PerteneceTablero(idUsuario, idTablero))
            {
                ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(_usuarioRepository.GetAllUsuarios());
                ModificarTareaViewModel tarea = new ModificarTareaViewModel(_tareaRepository.GetTareaById(idTarea), usuarios);
                return View(tarea);
            } else
            {
                if(isOperador())
                {
                    return RedirectToAction("ModificarTareaOperador", new {idTarea = idTarea});
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
    public IActionResult ModificarTareaOperador(int idTarea)
    {
        try
        {
            if(isOperador())
            {
                ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(_usuarioRepository.GetAllUsuarios());
                ModificarTareaViewModel tarea = new ModificarTareaViewModel(_tareaRepository.GetTareaById(idTarea), usuarios);
                return View(tarea);
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
    public IActionResult UpdateTarea(ModificarTareaViewModel tareaModificadaVM)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToAction("Error"); 
                } else
                {
                    Tarea tareaModificada = new Tarea(tareaModificadaVM);
                    _tareaRepository.UpdateTarea(tareaModificada);
                    return RedirectToAction("ListarTareas");
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
    public IActionResult EliminarTarea(int idTarea)
    {
        try
        {
            if(isAdmin() || isOperador())
            {
                TareaViewModel tarea = new TareaViewModel(_tareaRepository.GetTareaById(idTarea));
                return View(tarea); 
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

    public IActionResult DeleteTarea(int idTarea)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                _tareaRepository.DeleteTarea(idTarea);
                return RedirectToAction("ListarTareas");
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
        if(HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Administrador")
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