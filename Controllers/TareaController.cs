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
    private ILogger<TareaController> _logger;

    public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository)
    {
        _logger = logger;
        _tareaRepository = tareaRepository;
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
    public IActionResult ListarTareas(int idTablero)
    {
        try
        {
            if(isAdmin())
            {
                ListarTareasViewModel tareas = new ListarTareasViewModel(_tareaRepository.GetAllTareas());
                return View(tareas);
            } else
            {
                ListarTareasViewModel tareas = new ListarTareasViewModel(_tareaRepository.GetAllTareasByUsuario(Int32.Parse(HttpContext.Session.GetString("Id")!)));
                return View(tareas);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());            
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult AltaTarea()
    {
        try
        {
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
           if(!ModelState.IsValid)
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            } else
            {
                Tarea tareaNueva = new Tarea(tareaNuevaVM.IdTablero, tareaNuevaVM.Nombre, tareaNuevaVM.Descripcion, tareaNuevaVM.Color, tareaNuevaVM.IdUsuarioAsignado.GetValueOrDefault(), tareaNuevaVM.Estado);
                _tareaRepository.CreateTarea(tareaNuevaVM.IdTablero, tareaNueva);
                return RedirectToAction("ListarTareas");
            } 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());            
            return RedirectToRoute(new {controller = "Home", action = "Index"}); // ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult ModificarTarea(int idTarea)
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
    public IActionResult UpdateTarea(ModificarTareaViewModel tareaModificadaVM)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            } else
            {
                Tarea tareaModificada = new Tarea(tareaModificadaVM.IdTablero, tareaModificadaVM.Nombre, tareaModificadaVM.Descripcion, tareaModificadaVM.Color,tareaModificadaVM.IdUsuarioAsignado.GetValueOrDefault(), tareaModificadaVM.Estado);
                _tareaRepository.UpdateTarea(tareaModificadaVM.Id, tareaModificada);
                return RedirectToAction("ListarTareas");
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}