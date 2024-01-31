using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;


public class TableroViewModel
{
    private int id;
    private int idUsuarioPropietario;
    private string nombre;
    private string? descripcion;
    private string nombreUsuario;

    public TableroViewModel()
    {
    }

    public TableroViewModel(Tablero tablero)
    {
        this.id = tablero.Id;
        this.idUsuarioPropietario = tablero.IdUsuarioPropietario;
        this.nombre = tablero.Nombre;
        this.descripcion = tablero.Descripcion;
    }

    public TableroViewModel(Tablero tablero, string nombreUsuario)
    {
        this.id = tablero.Id;
        this.idUsuarioPropietario = tablero.IdUsuarioPropietario;
        this.nombre = tablero.Nombre;
        this.descripcion = tablero.Descripcion;
        this.nombreUsuario = nombreUsuario;
    }

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
}