using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ModificarTableroViewModel
{
    private int id;
    private int idUsuarioPropietario;
    private string nombre;
    private string? descripcion;

    public ModificarTableroViewModel()
    {
    }

    public ModificarTableroViewModel(Tablero tableroModificado)
    {
        this.id = tableroModificado.Id;
        this.idUsuarioPropietario = tableroModificado.IdUsuarioPropietario;
        this.nombre = tableroModificado.Nombre;
        this.descripcion = tableroModificado.Descripcion;
    }


    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Id")]
    public int Id { get => id; set => id = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Id Usuario Propietario")]
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [MaxLength(10, ErrorMessage = "El nombre debe tener hasta 10 caracteres")]
    [Display(Name = "Nombre")]
    public string Nombre { get => nombre; set => nombre = value; }

    [MaxLength(50, ErrorMessage = "La descripcion debe tener hasta 50 caracteres")]
    [Display(Name = "Descripcion")]
    public string? Descripcion { get => descripcion; set => descripcion = value; }
}