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
    public List<Tarea> GetAllTareasByTablero(int idTablero);
    public List<TareaViewModel> GetTareasViewModel();
    public List<TareaViewModel> GetTareaViewModelByUsuario(int idTarea);
    public void AsignarUsuario(int idUsuario, int idTarea);
    public void DeleteTarea(int idTarea);
}