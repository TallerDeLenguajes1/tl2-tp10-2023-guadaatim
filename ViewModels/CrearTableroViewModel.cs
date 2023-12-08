using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class CrearTableroViewModel
{
    private int idUsuarioPropietario;
    private string? nombre;
    private string descripcion;

    public CrearTableroViewModel()
    {
    }

    public CrearTableroViewModel(TableroViewModel tableroNuevo)
    {
        this.idUsuarioPropietario = tableroNuevo.IdUsuarioPropietario;
        this.nombre = tableroNuevo.Nombre;
        this.descripcion = tableroNuevo.Descripcion;
    }

    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
}