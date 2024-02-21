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
    private ITableroRepository _tableroRepository;
    private ITareaRepository _tareaRepository;
    private ILogger<UsuarioController> _logger;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository, ITareaRepository tareaRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _tableroRepository = tableroRepository;
        _tareaRepository = tareaRepository;
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
                    return RedirectToAction("Error"); 
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
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
               return RedirectToAction("Error"); 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
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
                return RedirectToAction("Error"); 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
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
                    usuarioNuevoVM.Error = "Error al crear el usuario. Intente de nuevo.";
                    return View(usuarioNuevoVM); 
                } else 
                {
                    if(_usuarioRepository.ExisteUsuario(usuarioNuevoVM.NombreDeUsuario))
                    {
                        usuarioNuevoVM.Error = "El nombre de usuario ya existe.";
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
                return RedirectToAction("Error"); 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
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
                ModificarUsuarioViewModel usuario = new ModificarUsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
                if (id == idUsuario)
                {
                    return View("ModificarUsuarioAdmin", usuario);
                } else
                {
                    return View(usuario);
                }
            } else
            {
                if (isOperador())
                {
                    int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                    ModificarUsuarioViewModel usuario = new ModificarUsuarioViewModel(_usuarioRepository.GetUsuarioById(idUsuario));
                    return View("ModificarUsuarioOperador", usuario);
                }
                return RedirectToAction("Error"); 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
        }
    }

    [HttpPost]
    public IActionResult ModificarUsuario(ModificarUsuarioViewModel usuarioModificadoVM)
    {
        try
        {
            if (isAdmin() || isOperador())
            {
                if(!ModelState.IsValid)
                {   
                    usuarioModificadoVM.Error = "Error al modificar el usuario.";
                    if (isAdmin())
                    {
                        int id = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
                        if (id == usuarioModificadoVM.Id)
                        {
                            return View("ModificarUsuarioAdmin", usuarioModificadoVM);
                        } else
                        {
                            return View("ModificarUsuario", usuarioModificadoVM);
                        }
                    } else
                    {
                        return View("ModificarUsuarioOperador", usuarioModificadoVM);
                    }
                } else
                {
                    Usuario usuarioModificado = new Usuario(usuarioModificadoVM);
                    _usuarioRepository.UpdateUsuario(usuarioModificado);
                    return RedirectToAction("ListarUsuarios");
                }
            } else
            {
                return RedirectToAction("Error"); 
            }  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
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
                return RedirectToAction("Error"); 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
        }
    }

    public IActionResult DeleteUsuario(int idUsuario)
    {
        try
        {
            if(isAdmin())
            {
                _tareaRepository.UpdateTareaAsignada(idUsuario);
                _tareaRepository.DeleteByUsuario(idUsuario);
                _tableroRepository.DeleteTableroByUsuario(idUsuario);
                _usuarioRepository.DeleteUsuario(idUsuario);
                return RedirectToAction("ListarUsuarios");
            } else
            {
                return RedirectToAction("Error"); 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); 
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