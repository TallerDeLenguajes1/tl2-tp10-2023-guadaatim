using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;
using tl2_tp10_2023_guadaatim.Models;

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

    [HttpGet("GetAllTareasByTablero")]
    public IActionResult GetAll(int idTablero)
    {
        List<Tarea> tareas = tareaRepository.GetAllTareasByTablero(idTablero);
        
        if (tareas != null)
        {
            return View(tareas);
        } else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult CreateTarea(int idTablero, Tarea tareaNueva)
    {
        tareaRepository.CreateTarea(idTablero, tareaNueva);
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateTarea(int idTarea, Tarea tareaModificada)
    {
        tareaRepository.UpdateTarea(idTarea, tareaModificada);
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteTarea(int idTarea)
    {
        tareaRepository.DeleteTarea(idTarea);
        return Ok();
    }
}