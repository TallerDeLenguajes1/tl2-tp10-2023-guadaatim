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

    [Required(ErrorMessage = "Complete el campo")]
    [MaxLength(10, ErrorMessage = "El nombre debe tener hasta 10 caracteres")]
    [Display(Name = "Nombre de Usuario")]
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    
    [Required(ErrorMessage = "Complete el campo")]
    [Range(8, 16, ErrorMessage = "La contreseña debe tener entre 8 y 16 caracteres")]
    [Display(Name = "Contraseña")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    
    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Rol")]
    public Rol Rol { get => rol; set => rol = value; }
}