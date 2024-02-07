using Kanban.ViewModels;

namespace Kanban.Models;

public class Tablero
{
    private int id;
    private int idUsuarioPropietario;
    private string nombre;
    private string? descripcion;

    public Tablero()
    {
    }

    public Tablero(CrearTableroViewModel tableroVM)
    {
        this.idUsuarioPropietario = tableroVM.IdUsuarioPropietario;
        this.nombre = tableroVM.Nombre;
        this.descripcion = tableroVM.Descripcion;
    }

    public Tablero(ModificarTableroViewModel tableroVM)
    {
        this.id = tableroVM.Id;
        this.idUsuarioPropietario = tableroVM.IdUsuarioPropietario;
        this.nombre = tableroVM.Nombre;
        this.descripcion = tableroVM.Descripcion;
    }

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
}