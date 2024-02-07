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

    public ListarTareasViewModel(List<TareaViewModel> tareas)
    {
        tareasVM = new List<TareaViewModel>();

        foreach (var tarea in tareas)
        {
            tareasVM.Add(tarea);
        }
    }

    public ListarTareasViewModel(List<TareaViewModel> tareas, List<TareaViewModel> tareasPropias)
    {
        tareasVM = new List<TareaViewModel>();
        tareasPropiasVM = new List<TareaViewModel>();

        foreach (var tarea in tareasPropias)
        {
            tareasPropiasVM.Add(tarea);
        }

        foreach (var tarea in tareas)
        {
            tareasVM.Add(tarea);
        }
    }

    public List<TareaViewModel> TareasVM { get => tareasVM; set => tareasVM = value; }
    public List<TareaViewModel> TareasPropiasVM { get => tareasPropiasVM; set => tareasPropiasVM = value; }
}