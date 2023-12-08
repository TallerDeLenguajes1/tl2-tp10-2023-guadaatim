using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;


public class TableroViewModel
{
    private int id;
    private int idUsuarioPropietario;
    private string? nombre;
    private string descripcion;

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

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
}