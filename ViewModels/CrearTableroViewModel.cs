using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Kanban.Models;

namespace Kanban.ViewModels;

public class CrearTableroViewModel
{
    private int idUsuarioPropietario;
    private string nombre;
    private string? descripcion;

    public CrearTableroViewModel()
    {
    }

    public CrearTableroViewModel(TableroViewModel tableroNuevo)
    {
        this.idUsuarioPropietario = tableroNuevo.IdUsuarioPropietario;
        this.nombre = tableroNuevo.Nombre;
        this.descripcion = tableroNuevo.Descripcion;
    }

    [Required(ErrorMessage = "Complete el campo")]
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    
    [Required(ErrorMessage = "Complete el campo")]
    [MaxLength(10, ErrorMessage = "El nombre debe tener hasta 10 caracteres")]
    [Display(Name = "Nombre")]
    public string Nombre { get => nombre; set => nombre = value; }

    [MaxLength(50, ErrorMessage = "La descripcion debe tener hasta 50 caracteres")]
    [Display(Name = "Descripcion")]
    public string? Descripcion { get => descripcion; set => descripcion = value; }
}