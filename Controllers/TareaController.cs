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
                List<TareaViewModel> tareas = _tareaRepository.GetTareasViewModel();
                List<TareaViewModel> tareasPropias = _tareaRepository.GetTareasViewModelByUsuario(id);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, tareasPropias);
                return View(tareasVM);
            } else
            {
                if (isOperador())
                {
                    return RedirectToAction("ListarTareasOperador");
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
    public IActionResult ListarTareasOperador()
    {
        try
        {
            if(isOperador())
            {

                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                List<TareaViewModel> tareas = _tareaRepository.GetTareasViewModel();
                List<TareaViewModel> tareasPropias = _tareaRepository.GetTareasViewModelByUsuario(id);
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, tareasPropias);
                return View(tareasVM);
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
    public IActionResult MostrarTarea(int idTablero)
    {
        try
        {
            if (isOperador()) //operador y admin ?? 
            {
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                TareaViewModel tarea = _tareaRepository.GetTareaViewModel(id, idTablero);
                return View(tarea);
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
    public IActionResult ListarTareasPorTablero(int idTablero)
    {
        if (isAdmin())
        {
            ListarTareasViewModel tareas = new ListarTareasViewModel(_tareaRepository.GetAllTareasByTablero(idTablero), _usuarioRepository.GetAllUsuarios(), _tableroRepository.GetAllTableros(), HttpContext.Session.GetString("Rol")!, HttpContext.Session.GetString("NombreDeUsuario")!);
            return View(tareas);
        } else
        {
            if(isOperador())
            {
                ListarTareasViewModel tareas = new ListarTareasViewModel(_tareaRepository.GetAllTareasByTablero(idTablero), _usuarioRepository.GetAllUsuarios(), _tableroRepository.GetAllTableros(), HttpContext.Session.GetString("Rol")!, HttpContext.Session.GetString("NombreDeUsuario")!);
                return View(tareas); //otra vista q no pueda modificar tareas q no son de el usuario
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
            }
        }
    }

    [HttpGet]
    public IActionResult AltaTarea()
    {
        try
        {
            ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(_usuarioRepository.GetAllUsuarios());
            if(isAdmin())
            {
                return View(new CrearTareaViewModel());
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
    public IActionResult CreateTarea(CrearTareaViewModel tareaNuevaVM)
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
                    Tarea tareaNueva = new Tarea(tareaNuevaVM.IdTablero, tareaNuevaVM.Nombre, tareaNuevaVM.Descripcion, tareaNuevaVM.Color, Convert.ToInt32(HttpContext.Session.GetString("Id")), tareaNuevaVM.Estado);
                    _tareaRepository.CreateTarea(tareaNuevaVM.IdTablero, tareaNueva);
                    return RedirectToAction("ListarTareas");
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
    public IActionResult AsignarUsuario(int idTarea, string nombreUsuario)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                if(HttpContext.Session.GetString("NombreDeUsuario") == nombreUsuario)
                {
                    AsignarUsuarioViewModel usuarios = new AsignarUsuarioViewModel(_usuarioRepository.GetAllUsuarios(), _tareaRepository.GetTareaById(idTarea));
                    return View(usuarios);
                } else
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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
    public IActionResult AssignUsuario(int idTarea, int idUsuario)
    {
        try
        {
            if(isAdmin() || isOperador())
            {
                if(Convert.ToInt32(HttpContext.Session.GetString("Id")) != idUsuario)
                {
                    _tareaRepository.AsignarUsuario(idUsuario, idTarea);
                    return RedirectToAction("ListarTareas");
                } else
                {
                    return RedirectToAction("ListarTareas");
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
    public IActionResult ModificarTarea(int idTarea, string nombreUsuario)
    {
        try
        {
            if(isAdmin())
            {
                string nombreSession =  HttpContext.Session.GetString("NombreDeUsuario");
                ModificarTareaViewModel tarea = new ModificarTareaViewModel(_tareaRepository.GetTareaById(idTarea));
                return View(tarea);
            } else
            {
                if(isOperador())
                {
                    return RedirectToAction("ModificarTareaOperador", new {idTarea = idTarea});
                } else
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
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
    public IActionResult ModificarTareaOperador(int idTarea, string nombreUsuario)
    {
        try
        {
            if(isOperador())
            {
                string nombreSession =  HttpContext.Session.GetString("NombreDeUsuario");
                ModificarTareaViewModel tarea = new ModificarTareaViewModel(_tareaRepository.GetTareaById(idTarea));
                return View(tarea);
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
    public IActionResult UpdateTarea(ModificarTareaViewModel tareaModificadaVM)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                if(!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else
                {
                    Tarea tareaModificada = new Tarea(tareaModificadaVM.Id, tareaModificadaVM.IdTablero, tareaModificadaVM.Nombre, tareaModificadaVM.Descripcion, tareaModificadaVM.Color, tareaModificadaVM.IdUsuarioAsignado.GetValueOrDefault(), tareaModificadaVM.Estado);
                    _tareaRepository.UpdateTarea(tareaModificada);
                    return RedirectToAction("ListarTareas");
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
    public IActionResult EliminarTarea(int idTarea)
    {
        try
        {
            if(isAdmin())
            {
                Tarea tarea = _tareaRepository.GetTareaById(idTarea);
                return View(tarea); 
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
    public IActionResult DeleteTarea(Tarea tarea)
    {
        try
        {
            _tareaRepository.DeleteTarea(tarea.Id);
            return RedirectToAction("ListarTareas");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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