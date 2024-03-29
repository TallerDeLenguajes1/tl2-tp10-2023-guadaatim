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
    private string? error;

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

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Id")]
    public int Id { get => id; set => id = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [MaxLength(10, ErrorMessage = "El nombre debe tener hasta 10 caracteres")]
    [Display(Name = "Nombre de Usuario")]
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [StringLength(16, ErrorMessage = "La contraseña debe tener entre 8 y 16 caracteres"), MinLength(8)]
    [Display(Name = "Contraseña")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Rol")]
    public Rol Rol { get => rol; set => rol = value; }
    public string? Error { get => error; set => error = value; }
}