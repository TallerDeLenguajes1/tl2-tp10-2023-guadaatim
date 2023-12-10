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
    private IUsuarioRepository _usuarioRepository;

    public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
    }

    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    
    public IActionResult Login(Usuario usuario)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index"});   
            } else
            {
                try
                {
                    List<Usuario> usuarios = _usuarioRepository.GetAllUsuarios();
                    Usuario usuarioLoggeado = usuarios.FirstOrDefault(u => u.NombreDeUsuario == usuario.NombreDeUsuario && u.Contrasenia == usuario.Contrasenia);

                    if (usuarioLoggeado == null)
                    {
                        _logger.LogWarning("Intento de acceso invalido - Usuario: " + usuario.NombreDeUsuario + " - Clave ingresada: " + usuario.Contrasenia);
                        return RedirectToAction("Index");
                    } else
                    {
                        loggearUsuario(usuarioLoggeado);
                        _logger.LogInformation("El usuario " + usuario.NombreDeUsuario + " ingreso correctamente!");
                        return RedirectToRoute(new { controller = "Login", action = "Index"});
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return RedirectToRoute(new { controller = "Login", action = "Index"});
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new { controller = "Login", action = "Index"});
        }
    }

    private void loggearUsuario(Usuario usuario)
    {
        HttpContext.Session.SetString("NombreDeUsuario", usuario.NombreDeUsuario);
        HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
        HttpContext.Session.SetString("Id", usuario.Id.ToString());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}