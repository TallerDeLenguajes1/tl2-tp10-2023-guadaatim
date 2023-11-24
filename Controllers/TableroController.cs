using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;

namespace Kanban.Controllers;

public class TableroController : Controller
{
    private ITableroRepository tableroRepository;
    private readonly ILogger<TableroController> _logger;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepository = new TableroRepository();
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
        List<Tablero> tableros = tableroRepository.GetAllTableros();

        if(tableros != null)
        {
            return View(tableros);
        } else
        {
            return Error();
        }
    }

    [HttpGet]
    public IActionResult AltaTablero()
    {
       return View(new Tablero());
    }

    [HttpPost]
    public IActionResult CreateTablero(Tablero tableroNuevo)
    {
        tableroRepository.CreateTablero(tableroNuevo);
        return Ok();
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTablero)
    {
        Tablero tablero = tableroRepository.GetTableroById(idTablero);
        return View(tablero);
    }

    [HttpPost]
    public IActionResult UpdateTablero(Tablero tableroModificado)
    {
        tableroRepository.UpdateTablero(tableroModificado.Id, tableroModificado);
        return RedirectToAction("GetAllTableros");
    }

    [HttpGet]
    public IActionResult EliminarTablero(int idTablero)
    {
        Tablero tablero = tableroRepository.GetTableroById(idTablero);
        return View(tablero);
    }

    [HttpPost]
    public IActionResult DeleteTablero(Tablero tablero)
    {
        tableroRepository.DeleteTablero(tablero.Id);
        return RedirectToAction("GetAllTableros");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}