using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ModificarTableroViewModel
{
    private int id;
    private int idUsuarioPropietario;
    private string? nombre;
    private string descripcion;

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

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
}