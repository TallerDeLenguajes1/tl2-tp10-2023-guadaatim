using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class UsuarioViewModel
{
    private int id;
    private string nombreDeUsuario;
    private Rol rol;

    public UsuarioViewModel()
    {
    }

    public UsuarioViewModel(Usuario usuario)
    {
        this.id = usuario.Id;
        this.nombreDeUsuario = usuario.NombreDeUsuario;
        this.rol = usuario.Rol;
    }

    public int Id { get => id; set => id = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public Rol Rol { get => rol; set => rol = value; }
}