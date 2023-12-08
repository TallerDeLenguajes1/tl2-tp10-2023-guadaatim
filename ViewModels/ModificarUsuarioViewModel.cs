using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ModificarUsuarioViewModel
{
    private int id;
    private string nombreDeUsuario;
    private string contrasenia;
    private Rol rol;

    public ModificarUsuarioViewModel()
    {
    }

    public ModificarUsuarioViewModel(Usuario usuarioModificado)
    {
        id = usuarioModificado.Id;
        nombreDeUsuario = usuarioModificado.NombreDeUsuario;
        contrasenia = usuarioModificado.Contrasenia;
        rol = usuarioModificado.Rol;
    }

    public int Id { get => id; set => id = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    public Rol Rol { get => rol; set => rol = value; }
}