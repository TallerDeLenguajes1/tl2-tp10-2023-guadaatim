using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

public class CrearUsuarioViewModel
{
    [Required(ErrorMessage = "Complete el campo")]
    private int id;

    [Required(ErrorMessage = "Complete el campo")]
    private string nombreDeUsuario;

    [Required(ErrorMessage = "Complete el campo")]
    private string contrasenia;

    [Required(ErrorMessage = "Complete el campo")]
    private Rol rol;

    public CrearUsuarioViewModel()
    {
    }

    public CrearUsuarioViewModel(Usuario usuarioNuevo)
    {
        this.id = usuarioNuevo.Id;
        this.nombreDeUsuario = usuarioNuevo.NombreDeUsuario;
        this.contrasenia = usuarioNuevo.Contrasenia;
        this.rol = usuarioNuevo.Rol;
    }

    public int Id { get => id; set => id = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    public Rol Rol { get => rol; set => rol = value; }
}