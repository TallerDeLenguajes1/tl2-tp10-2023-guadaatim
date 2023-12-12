using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;
using Kanban.ViewModels;

using tl2_tp10_2023_guadaatim.Models;

namespace Kanban.Controllers;

public class UsuarioController : Controller
{
    private IUsuarioRepository _usuarioRepository;
    private ILogger<UsuarioController> _logger;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
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
        try
        {
            ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(_usuarioRepository.GetAllUsuarios());

            if (isAdmin())
            {
                return View(usuarios);
            } else
            {
                if (HttpContext.Session.GetString("Rol") == "Operador")
                {
                    return RedirectToAction("ListarUsuariosOperador");
                } else
                {
                    return RedirectToRoute(new {controller = "Login", action = "Index"});
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet] // mostrar el mismo usuario
    public IActionResult ListarUsuariosOperador()
    {
        try
        {
            UsuarioViewModel usuario = new UsuarioViewModel(_usuarioRepository.GetUsuarioById(Int32.Parse(HttpContext.Session.GetString("Id")!)));

            if(HttpContext.Session.GetString("Rol") == "Operador")
            {
                return View(usuario);
            } else
            {
                return RedirectToRoute(new {controller = "Login", action = "Index"});
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult AltaUsuario()
    {
        try
        {
            if (isAdmin())
            { 
                return View(new CrearUsuarioViewModel());
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR 
        }
    }

    [HttpPost]
    public IActionResult CreateUsuario(CrearUsuarioViewModel usuarioNuevoVM)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            } else 
            {
                Usuario usuarioNuevo = new Usuario(usuarioNuevoVM.NombreDeUsuario, usuarioNuevoVM.Contrasenia, usuarioNuevoVM.Rol);
                _usuarioRepository.CreateUsuario(usuarioNuevo);
                return RedirectToAction("ListarUsuarios");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuario)
    {
        try
        {
            if(isAdmin())
            {
                ModificarUsuarioViewModel usuario = new ModificarUsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
                return View(usuario);
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpPost]
    public IActionResult UpdateUsuario(ModificarUsuarioViewModel usuarioModificadoVM)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            } else
            {
                Usuario usuarioModificado = new Usuario(usuarioModificadoVM.NombreDeUsuario, usuarioModificadoVM.Contrasenia, usuarioModificadoVM.Rol);
                _usuarioRepository.UpdateUsuario(usuarioModificado.Id, usuarioModificado);
                return RedirectToAction("ListarUsuarios");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpGet]
    public IActionResult EliminarUsuario(int idUsuario)
    {
        try
        {
            if(isAdmin())
            {
                Usuario usuario = _usuarioRepository.GetUsuarioById(idUsuario);
                return View(usuario);
            } else
            {
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
        }
    }

    [HttpPost]
    public IActionResult DeleteUsuario(Usuario usuario) // agregar validacion ???
    {
        try
        {
            _usuarioRepository.DeleteUsuario(usuario.Id);
            return RedirectToAction("ListarUsuarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
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