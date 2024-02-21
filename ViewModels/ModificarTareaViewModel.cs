using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Kanban.ViewModels;

public class ModificarTareaViewModel
{ //idusuario acepta null??
    private int id;
    private int idTablero;
    private string nombre;
    private EstadoTarea estado;
    private string? descripcion;
    private string? color;
    private int idUsuarioAsignado;
    private string? error;
    private ListarUsuariosViewModel? usuarios;

    public ModificarTareaViewModel()
    {
    }

    public ModificarTareaViewModel(Tarea tarea, ListarUsuariosViewModel usuarios)
    {
        this.id = tarea.Id;
        this.idTablero = tarea.IdTablero;
        this.nombre = tarea.Nombre;
        this.estado = tarea.Estado;
        this.descripcion = tarea.Descripcion;
        this.color = tarea.Color;
        this.idUsuarioAsignado = tarea.IdUsuarioAsignado;
        this.usuarios = usuarios;
    }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Id")]
    public int Id { get => id; set => id = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Id Tablero")]
    public int IdTablero { get => idTablero; set => idTablero = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [MaxLength(15, ErrorMessage = "El nombre debe tener hasta 15 caracteres")]
    [Display(Name = "Nombre")]
    public string Nombre { get => nombre; set => nombre = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Estado Tarea")]
    public EstadoTarea Estado { get => estado; set => estado = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [MaxLength(50, ErrorMessage = "La descripcion debe tener hasta 50 caracteres")]
    [Display(Name = "Descripcion")]
    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Color")]
    public string Color { get => color; set => color = value; }

    [Required(ErrorMessage = "Este campo no puede estar vacio")]
    [Display(Name = "Id Usuario Asignado")]
    public int IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    public ListarUsuariosViewModel? Usuarios { get => usuarios; set => usuarios = value; }
    public string? Error { get => error; set => error = value; }
}