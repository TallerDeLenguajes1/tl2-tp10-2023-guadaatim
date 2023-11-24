using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;

namespace Kanban.Controllers;

public class UsuarioController : Controller
{
    private IUsuarioRepository usuarioRepository;
    private ILogger<UsuarioController> _logger;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        usuarioRepository = new UsuarioRepository();
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
    public IActionResult ListarUsuarios()
    {
        List<Usuario> usuarios = usuarioRepository.GetAllUsuarios();

        if (usuarios != null)
        {
            return View(usuarios);
        } else
        {
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult AltaUsuario()
    {
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult CreateUsuario(Usuario usuarioNuevo)
    {
        usuarioRepository.CreateUsuario(usuarioNuevo);
        return RedirectToAction("ListarUsuarios");
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuario)
    {
        Usuario usuario = usuarioRepository.GetUsuarioById(idUsuario);
        return View(usuario);
    }

    [HttpPost]
    public IActionResult UpdateUsuario(Usuario usuarioModificado)
    {
        usuarioRepository.UpdateUsuario(usuarioModificado.Id, usuarioModificado);
        return RedirectToAction("ListarUsuarios");
    }

    [HttpGet]
    public IActionResult EliminarUsuario(int idUsuario)
    {
        Usuario usuario = usuarioRepository.GetUsuarioById(idUsuario);
        return View(usuario);
    }

    [HttpPost]
    public IActionResult DeleteUsuario(Usuario usuario)
    {
        usuarioRepository.DeleteUsuario(usuario.Id);
        return RedirectToAction("ListarUsuarios");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}