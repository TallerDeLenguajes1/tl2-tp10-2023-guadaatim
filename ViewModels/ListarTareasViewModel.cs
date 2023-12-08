using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ListarTareasViewModel
{
    List<TareaViewModel> tareasVM;

    public ListarTareasViewModel()
    {
    }

    public ListarTareasViewModel(List<Tarea> tareas)
    {
        foreach (var t in tareas)
        {
            TareaViewModel tarea = new TareaViewModel(t);
            tareasVM.Add(tarea);
        }
    }
    public List<TareaViewModel> TareasVM { get => tareasVM; set => tareasVM = value; }
}