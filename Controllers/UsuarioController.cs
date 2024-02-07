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
    
    [HttpGet]
    public IActionResult ListarUsuarios()
    {
        try
        {
            if (isAdmin())
            {
                int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                List<Usuario> usuarios = _usuarioRepository.GetAllUsuariosExcept(idUsuario);
                UsuarioViewModel usuarioVM = new UsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
                ListarUsuariosViewModel usuariosVM = new ListarUsuariosViewModel(usuarios, usuarioVM);
                return View(usuariosVM);
            } else
            {
                if (isOperador())
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

    [HttpGet]
    public IActionResult ListarUsuariosOperador()
    {
        try
        {
            if(isOperador())
            {
                Usuario usuario = _usuarioRepository.GetUsuarioByNombre(HttpContext.Session.GetString("NombreDeUsuario")!);
                UsuarioViewModel usuarioVM = new UsuarioViewModel(usuario);
                return View(usuarioVM);
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
    public IActionResult CrearUsuario()
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
    public IActionResult CrearUsuario(CrearUsuarioViewModel usuarioNuevoVM)
    {
        try
        {
            if (isAdmin())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else 
                {
                    if(_usuarioRepository.ExisteUsuario(usuarioNuevoVM.NombreDeUsuario))
                    {
                        ModelState.AddModelError("NombreDeUsuario", "El nombre de usuario no esta disponible");
                        return View(usuarioNuevoVM);
                    } else
                    {
                        Usuario usuarioNuevo = new Usuario(usuarioNuevoVM);
                        _usuarioRepository.CreateUsuario(usuarioNuevo);
                        return RedirectToAction("ListarUsuarios");
                    }
                }
            } else
            {
                return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
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
                int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                if (id == idUsuario)
                {
                    ModificarUsuarioViewModel usuario = new ModificarUsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
                    return View("ModificarUsuarioAdmin", usuario);
                } else
                {
                    ModificarUsuarioViewModel usuario = new ModificarUsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
                    return View(usuario);
                }
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
    public IActionResult ModificarUsuario(ModificarUsuarioViewModel usuarioModificadoVM)
    {
        try
        {
            if (isAdmin())
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToRoute(new {controller = "Home", action = "Index"});
                } else
                {
                    Usuario usuarioModificado = new Usuario(usuarioModificadoVM);
                    _usuarioRepository.UpdateUsuario(usuarioModificado);
                    return RedirectToAction("ListarUsuarios");
                }
            } else
            {
                return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
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
                UsuarioViewModel usuario = new UsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
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
    public IActionResult DeleteUsuario(UsuarioViewModel usuario)
    {
        try
        {
            if(isAdmin())
            {
                _usuarioRepository.DeleteUsuario(usuario.Id);
                return RedirectToAction("ListarUsuarios");
            } else
            {
                return RedirectToRoute(new {controller = "Login", action = "Index"}); //ENVIAR A PAGINA DE ERROR
            }
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

    private bool isOperador()
    {
        if(HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Operador")
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