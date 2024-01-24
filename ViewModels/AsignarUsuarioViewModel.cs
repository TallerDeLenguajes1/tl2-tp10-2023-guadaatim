using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class AsignarUsuarioViewModel
{
    private ListarUsuariosViewModel usuarios;
    private TareaViewModel tarea;

    public AsignarUsuarioViewModel()
    {
    }

    public AsignarUsuarioViewModel(List<Usuario> usuarios, Tarea tarea)
    {
        this.usuarios = new ListarUsuariosViewModel(usuarios);
        this.tarea = new TareaViewModel(tarea);
    }

    public ListarUsuariosViewModel Usuarios { get => usuarios; set => usuarios = value; }
    public TareaViewModel Tarea { get => tarea; set => tarea = value; }
}