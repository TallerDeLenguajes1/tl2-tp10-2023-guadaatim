using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Repository;

public interface ITareaRepository
{
    public void CreateTarea(int idTablero, Tarea tarea);
    public void UpdateTarea(Tarea tareaModificar);
    public List<Tarea> GetAllTareas();
    public Tarea GetTareaById(int idTarea);
    public Tarea GetTareaByUsuarioAndTablero(int idUsuario, int idTablero);
    public List<Tarea> GetAllTareasByUsuario(int idUsuario);
    public List<Tarea> GetAllTareasByTablero(int idTablero);
    public void DeleteTarea(int idTarea);
}