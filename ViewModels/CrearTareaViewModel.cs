using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Kanban.ViewModels;

public class CrearTareaViewModel
{
    private int idTablero;
    private string nombre;
    private EstadoTarea estado;
    private string? descripcion;
    private string? color;
    private int idUsuarioAsignado;
    private string? error;
    private ListarUsuariosViewModel? usuarios;

    public CrearTareaViewModel()
    {
    }

    public CrearTareaViewModel(int idTablero, ListarUsuariosViewModel usuarios)
    {
        this.idTablero = idTablero;
        this.Usuarios = usuarios;
    }

    public CrearTareaViewModel(Tarea tarea)
    {
        this.idTablero = tarea.IdTablero;
        this.nombre = tarea.Nombre;
        this.estado = tarea.Estado;
        this.descripcion = tarea.Descripcion;
        this.color = tarea.Color;
        this.IdUsuarioAsignado = tarea.IdUsuarioAsignado;
    }

    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Id Tablero")]
    public int IdTablero { get => idTablero; set => idTablero = value; }

    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Nombre")]
    [MaxLength(15, ErrorMessage = "El nombre debe tener hasta 15 caracteres")]
    public string Nombre { get => nombre; set => nombre = value; }

    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Estado Tarea")]
    public EstadoTarea Estado { get => estado; set => estado = value; }
    
    [MaxLength(50, ErrorMessage = "La descripcion debe tener hasta 50 caracteres")]
    [Display(Name = "Descripcion")]
    public string? Descripcion { get => descripcion; set => descripcion = value; }

    [Required(ErrorMessage = "Complete el campo")]
    [Display(Name = "Color")]
    public string? Color { get => color; set => color = value; }

    [Display(Name = "Id Usuario Asignado")]
    public int IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    public ListarUsuariosViewModel? Usuarios { get => usuarios; set => usuarios = value; }
    public string? Error { get => error; set => error = value; }
}