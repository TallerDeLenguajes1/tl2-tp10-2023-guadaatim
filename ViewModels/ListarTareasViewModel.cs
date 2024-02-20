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

    public ListarTareasViewModel(List<Tarea> tareas)
    {
        tareasVM = new List<TareaViewModel>();

        foreach (var tarea in tareas)
        {
            TareaViewModel tareaVM = new TareaViewModel(tarea);
            tareasVM.Add(tareaVM);
        }
    }

    public ListarTareasViewModel(List<Tarea> tareas, List<Tarea> tareasPropias)
    {
        tareasVM = new List<TareaViewModel>();
        tareasPropiasVM = new List<TareaViewModel>();

        foreach (var tarea in tareas)
        {
            TareaViewModel tareaVM = new TareaViewModel(tarea);
            tareasVM.Add(tareaVM);
        }

        foreach (var tarea in tareasPropias)
        {
            TareaViewModel tareaVM = new TareaViewModel(tarea);
            tareasPropiasVM.Add(tareaVM);
        }
    }

    public List<TareaViewModel> TareasVM { get => tareasVM; set => tareasVM = value; }
    public List<TareaViewModel> TareasPropiasVM { get => tareasPropiasVM; set => tareasPropiasVM = value; }
}