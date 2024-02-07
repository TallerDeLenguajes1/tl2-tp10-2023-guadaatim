using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Repository;

public interface ITareaRepository
{
    public void CreateTarea(int idTablero, Tarea tarea);
    public void UpdateTarea(Tarea tareaModificar);
    public List<Tarea> GetAllTareas();
    public Tarea GetTareaById(int idTarea);
    public List<Tarea> GetAllTareasByUsuario(int idUsuario);
    public List<TareaViewModel> GetAllTareasByTablero(int idTablero);
    public List<TareaViewModel> GetTareasViewModel();
    public List<TareaViewModel> GetTareasViewModelByUsuario(int idUsuario);
    public TareaViewModel GetTareaViewModel(int idUsuario, int idTablero);
    public void DeleteTarea(int idTarea);
}