using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Kanban.Repository;
using Kanban.Models;

using tl2_tp10_2023_guadaatim.Models;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private IUsuarioRepository usuarioRepository;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
        usuarioRepository = new UsuarioRepository();
    }

    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login(Usuario usuario)
    {
        List<Usuario> usuarios = usuarioRepository.GetAllUsuarios();
        Usuario usuarioLoggeado = usuarios.FirstOrDefault(u => u.NombreDeUsuario == usuario.NombreDeUsuario && u.Contrasenia == usuario.Contrasenia);

        if (usuarioLoggeado == null)
        {
            return RedirectToAction("Index");
        } else
        {
            loggearUsuario(usuarioLoggeado);
            return RedirectToRoute(new { controller = "Home", action = "Index"});
        }
    }

    private void loggearUsuario(Usuario usuario)
    {
        HttpContext.Session.SetString("NombreDeUsuario", usuario.NombreDeUsuario);
        HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}