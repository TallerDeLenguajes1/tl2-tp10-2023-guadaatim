using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class TareaViewModel
{
    private int id;
    private int idTablero;
    private string nombre;
    private EstadoTarea estado;
    private string? descripcion;
    private string? color;
    private int? idUsuarioAsignado;

    public TareaViewModel()
    {
    }

    public TareaViewModel(Tarea tarea)
    {
        this.id = tarea.Id;
        this.idTablero = tarea.IdTablero;
        this.nombre = tarea.Nombre;
        this.estado = tarea.Estado;
        this.color = tarea.Color;
        this.IdUsuarioAsignado = tarea.IdUsuarioAsignado;
    }

    public int Id { get => id; set => id = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    public string? Color { get => color; set => color = value; }
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
}