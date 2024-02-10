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
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                List<Tablero> tableros = _tableroRepository.GetAllTableros();
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, tareasPropias, usuarios, tableros);
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
                List<Tarea> tareas = _tareaRepository.GetAllTareas();
                List<Tarea> tareasPropias = _tareaRepository.GetAllTareasByUsuario(id);
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                List<Tablero> tableros = _tableroRepository.GetAllTableros();
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, tareasPropias, usuarios, tableros);
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
                Tarea tarea = _tareaRepository.GetTareaById(id);
                TareaViewModel tareaVM = new TareaViewModel(tarea);
                List<Tablero> tableros = _tableroRepository.GetAllTableros();
                tareaVM.NombreTablero = tableros.FirstOrDefault(t => t.Id == tareaVM.IdTablero).Nombre;
                return View(tareaVM);
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
        try
        {
            int idUsuario = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
            if (isAdmin() || _tableroRepository.PerteneceTablero(idUsuario, idTablero))
            {
                List<Tarea> tareas = _tareaRepository.GetAllTareasByTablero(idTablero);
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                List<Tablero> tableros = _tableroRepository.GetAllTableros();
                ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, usuarios, tableros);
                return View(tareasVM);
            } else
            {
                if(isOperador())
                {
                    return RedirectToAction("ListarTareasPorTableroOperador", new {idTablero = idTablero});
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
    public IActionResult ListarTareasPorTableroOperador(int idTablero)
    {
        if (isOperador())
        {
            List<Tarea> tareas = _tareaRepository.GetAllTareasByTablero(idTablero);
            List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
            List<Tablero> tableros = _tableroRepository.GetAllTableros();
            ListarTareasViewModel tareasVM = new ListarTareasViewModel(tareas, usuarios, tableros);
            return View(tareasVM);
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
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
                    Tarea tareaNueva = new Tarea(tareaNuevaVM);
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
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else
                {
                    Tarea tareaModificada = new Tarea(tareaModificadaVM);
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
            if(isAdmin() || isOperador())
            {
                TareaViewModel tarea = new TareaViewModel(_tareaRepository.GetTareaById(idTarea));
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
    public IActionResult DeleteTarea(TareaViewModel tarea)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                _tareaRepository.DeleteTarea(tarea.Id);
                return RedirectToAction("ListarTareas");
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