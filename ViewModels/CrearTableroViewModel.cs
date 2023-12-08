using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class CrearTableroViewModel
{
    [Required(ErrorMessage = "Complete el campo")]
    private int id;

    [Required(ErrorMessage = "Complete el campo")]
    private int idUsuarioPropietario;

    [Required(ErrorMessage = "Complete el campo")]
    private string? nombre;
    private string descripcion;

    public CrearTableroViewModel()
    {
    }

    public CrearTableroViewModel(TableroViewModel tableroNuevo)
    {
        this.id = tableroNuevo.Id;
        this.idUsuarioPropietario = tableroNuevo.IdUsuarioPropietario;
        this.nombre = tableroNuevo.Nombre;
        this.descripcion = tableroNuevo.Descripcion;
    }

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
}