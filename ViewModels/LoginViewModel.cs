using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Nombre de usuario")]
    public string NombreDeUsuario {get; set;}

    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Contraseña")]
    [PasswordPropertyText]
    public string Contrasenia {get; set;}
}
