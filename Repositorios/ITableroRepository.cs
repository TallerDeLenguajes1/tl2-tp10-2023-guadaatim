using Kanban.Models;

namespace Kanban.Repository;

public interface ITableroRepository
{
    public void CreateTablero(Tablero tablero);
    public void UpdateTablero(Tablero tableroModificar);
    public Tablero GetTableroById(int idTablero);
    public bool PerteneceTablero(int idUsuario, int idTablero);
    public List<Tablero> GetTableroByUsuario(int idUsuario);
    public List<Tablero> GetAllTableros();
    public List<Tablero> GetTablerosByTarea(int idUsuario);
    public void DeleteTablero(int idTablero);
    public void DeleteTableroByUsuario(int idUsuario);
}