using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Repository;
using Kanban.Models;
using Kanban.ViewModels;

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
        ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(usuarioRepository.GetAllUsuarios());

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

    [HttpGet] // mostrar el mismo usuario
    public IActionResult ListarUsuariosOperador()
    {
        //ListarUsuariosViewModel usuarios = new ListarUsuariosViewModel(usuarioRepository.GetAllUsuarios());

        UsuarioViewModel usuario = new UsuarioViewModel(usuarioRepository.GetUsuarioById(Int32.Parse(HttpContext.Session.GetString("Id")!)));

        if(HttpContext.Session.GetString("Rol") == "Operador")
        {
            return View(usuario);
        } else
        {
            return RedirectToRoute(new {controller = "Login", action = "Index"});
        }
    }

    [HttpGet] //donde va si no es admin
    public IActionResult AltaUsuario()
    {
        if (isAdmin())
        { 
            return View(new CrearUsuarioViewModel());
        } else
        {
            return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult CreateUsuario(CrearUsuarioViewModel usuarioNuevoVM)
    {
        Usuario usuarioNuevo = new Usuario(usuarioNuevoVM.NombreDeUsuario, usuarioNuevoVM.Contrasenia, usuarioNuevoVM.Rol);
        usuarioRepository.CreateUsuario(usuarioNuevo);
        return RedirectToAction("ListarUsuarios");
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuario)
    {
        if(isAdmin())
        {
            ModificarUsuarioViewModel usuario = new ModificarUsuarioViewModel(usuarioRepository.GetUsuarioById(idUsuario));
            return View(usuario);
        } else
        {
             return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult UpdateUsuario(ModificarUsuarioViewModel usuarioModificadoVM)
    {
        Usuario usuarioModificado = new Usuario(usuarioModificadoVM.NombreDeUsuario, usuarioModificadoVM.Contrasenia, usuarioModificadoVM.Rol);
        usuarioRepository.UpdateUsuario(usuarioModificado.Id, usuarioModificado);
        return RedirectToAction("ListarUsuarios");
    }

    [HttpGet]
    public IActionResult EliminarUsuario(int idUsuario)
    {
        if(isAdmin())
        {
            Usuario usuario = usuarioRepository.GetUsuarioById(idUsuario);
            return View(usuario);
        } else
        {
             return RedirectToRoute(new {controller = "Home", action = "Index"});
        }
    }

    [HttpPost]
    public IActionResult DeleteUsuario(Usuario usuario)
    {
        usuarioRepository.DeleteUsuario(usuario.Id);
        return RedirectToAction("ListarUsuarios");
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