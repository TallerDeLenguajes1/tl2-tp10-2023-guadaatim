using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class TareaController : Controller
{
    private ITareaRepository tareaRepository;
    private ILogger<TareaController> _logger;

    public TareaController(ILogger<TareaController> logger)
    {
        _logger = logger;
        tareaRepository = new TareaRepository();
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
        if(isAdmin())
        {
            ListarTareasViewModel tareas = new ListarTareasViewModel(tareaRepository.GetAllTareas());
            return View(tareas);
        } else
        {
            ListarTareasViewModel tareas = new ListarTareasViewModel(tareaRepository.GetAllTareasByUsuario(Int32.Parse(HttpContext.Session.GetString("Id")!)));
            return View(tareas);
        }
    }

    [HttpGet]
    public IActionResult AltaTarea()
    {
        if(isAdmin())
        {
            return View(new TareaViewModel());
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult CreateTarea(Tarea tareaNueva)
    {
        tareaRepository.CreateTarea(1, tareaNueva);
        return RedirectToAction("ListarTareas");
    }

    [HttpGet]
    public IActionResult ModificarTarea(int idTarea)
    {
        if(isAdmin())
        {
            Tarea tarea = tareaRepository.GetTareaById(idTarea);
            return View(tarea);
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
        
    }

    [HttpPost]
    public IActionResult UpdateTarea(Tarea tareaModificada)
    {
        tareaRepository.UpdateTarea(tareaModificada.Id, tareaModificada);
        return RedirectToAction("ListarTareas");
    }

    [HttpGet]
    public IActionResult EliminarTarea(int idTarea)
    {
        if(isAdmin())
        {
           Tarea tarea = tareaRepository.GetTareaById(idTarea);
            return View(tarea); 
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }   
    }

    [HttpPost]
    public IActionResult DeleteTarea(Tarea tarea) //enviar solo el id??
    {
        tareaRepository.DeleteTarea(tarea.Id);
        return RedirectToAction("ListarTareas");
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