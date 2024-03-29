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
                    bool existeUsuario = _usuarioRepository.ExisteUsuarioLogin(usuario);

                    if (!existeUsuario)
                    {
                        LoginViewModel loginVM = new LoginViewModel();
                        loginVM.Error = "Usuario o contraseña incorrectos. Intente otra vez";
                        _logger.LogWarning("Intento de acceso invalido - Usuario: " + usuario.NombreDeUsuario + " - Clave ingresada: " + usuario.Contrasenia);                        
                        return View("Index", loginVM);
                    } else
                    {
                        Usuario usuarioLoggeado = _usuarioRepository.GetUsuarioByNombre(usuario.NombreDeUsuario);
                        loggearUsuario(usuarioLoggeado);
                        _logger.LogInformation("El usuario " + usuario.NombreDeUsuario + " ingreso correctamente!");
                        return RedirectToRoute(new { controller = "Home", action = "Index"});
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

    public IActionResult Logout()
    {
        try
        {
            desloguearUsuario();
        }
        catch (Exception ex)
        {
            
            _logger.LogError("Error al intentar cerrar sesion del usuario" + ex.ToString());
        }
        return RedirectToAction("Index");
    }

    private void loggearUsuario(Usuario usuario)
    {
        HttpContext.Session.SetString("NombreDeUsuario", usuario.NombreDeUsuario);
        HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
        HttpContext.Session.SetInt32("Id", usuario.Id);
    }

    private void desloguearUsuario()
    {
        HttpContext.Session.Clear();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}