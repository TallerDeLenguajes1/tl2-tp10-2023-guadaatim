using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class CrearUsuarioViewModel
{
    private string nombreDeUsuario;
    private string contrasenia;
    private Rol rol;

    public CrearUsuarioViewModel()
    {
    }

    public CrearUsuarioViewModel(Usuario usuarioNuevo)
    {
        this.nombreDeUsuario = usuarioNuevo.NombreDeUsuario;
        this.contrasenia = usuarioNuevo.Contrasenia;
        this.rol = usuarioNuevo.Rol;
    }

    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    public Rol Rol { get => rol; set => rol = value; }
}