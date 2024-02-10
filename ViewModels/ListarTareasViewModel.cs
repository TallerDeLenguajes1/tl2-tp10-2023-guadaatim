using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ListarTareasViewModel
{
    private List<TareaViewModel> tareasVM;
    private List<TareaViewModel> tareasPropiasVM;

    public ListarTareasViewModel()
    {
    }

    public ListarTareasViewModel(List<Tarea> tareas, List<Usuario> usuarios, List<Tablero> tableros)
    {
        tareasVM = new List<TareaViewModel>();

        foreach (var tarea in tareas)
        {
            TareaViewModel tareaVM = new TareaViewModel(tarea);
            tareaVM.NombreTablero = tableros.FirstOrDefault(t => t.Id == tareaVM.IdTablero).Nombre;
            tareaVM.NombreUsuario = usuarios.FirstOrDefault(u => u.Id == tareaVM.IdUsuarioAsignado).NombreDeUsuario;
            tareasVM.Add(tareaVM);
        }
    }

    public ListarTareasViewModel(List<Tarea> tareas, List<Tarea> tareasPropias, List<Usuario> usuarios, List<Tablero> tableros)
    {
        tareasVM = new List<TareaViewModel>();
        tareasPropiasVM = new List<TareaViewModel>();

        foreach (var tarea in tareas)
        {
            TareaViewModel tareaVM = new TareaViewModel(tarea);
            tareaVM.NombreUsuario = usuarios.FirstOrDefault(u => u.Id == tareaVM.IdUsuarioAsignado).NombreDeUsuario;
            tareaVM.NombreTablero = tableros.FirstOrDefault(t => t.Id == tareaVM.IdTablero).Nombre;
            tareasVM.Add(tareaVM);
        }

        foreach (var tarea in tareasPropias)
        {
            TareaViewModel tareaVM = new TareaViewModel(tarea);
            tareaVM.NombreUsuario = usuarios.FirstOrDefault(u => u.Id == tareaVM.IdUsuarioAsignado).NombreDeUsuario;
            tareaVM.NombreTablero = tableros.FirstOrDefault(t => t.Id == tareaVM.IdTablero).Nombre;
            tareasPropiasVM.Add(tareaVM);
        }
    }

    public List<TareaViewModel> TareasVM { get => tareasVM; set => tareasVM = value; }
    public List<TareaViewModel> TareasPropiasVM { get => tareasPropiasVM; set => tareasPropiasVM = value; }
}