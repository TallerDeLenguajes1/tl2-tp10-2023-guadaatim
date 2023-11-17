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
    public IActionResult GetAllTableros()
    {
        List<Tablero> tableros = tableroRepository.GetAllTablero();

        if(tableros != null)
        {
            return View(tableros);
        } else
        {
            return Error();
        }
    }

    [HttpGet]
    public IActionResult Altatablero(Tablero tableroNuevo)
    {
       return View("AltaTablero");
    }

    [HttpPost]
    public IActionResult CreateTablero(Tablero tableroNuevo)
    {
        tableroRepository.CreateTablero(tableroNuevo);
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateTablero(int idTablero, Tablero tableroModificado)
    {
        
        tableroRepository.UpdateTablero(idTablero, tableroModificado);
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteTablero(int idTablero)
    {
        tableroRepository.DeleteTablero(idTablero);
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}