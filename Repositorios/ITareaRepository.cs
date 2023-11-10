using Kanban.Models;

namespace Kanban.Repository;

public interface ITareaRepository
{
    public void CreateTarea(int idTablero, Tarea tarea);
    public void UpdateTarea(int idTarea, Tarea tareaModificar);
    public Tarea GetTareaById(int idTarea);
    public List<Tarea> GetAllTareasByUsuario(int idUsuario);
    public List<Tarea> GetAllTareaByTablero(int idTablero);
    public void AsignarUsuario(int idUsuario, int idTarea);
    public void DeleteTarea(int idTarea);
}