using Kanban.Models;

namespace Kanban.Repository;

public interface ITableroRepository
{
    public void CreateTablero(Tablero tablero);
    public void UpdateTablero(Tablero tableroModificar);
    public List<Tablero> GetAllTableros();
    public List<TableroViewModel> GetTablerosViewModel();
    public List<TableroViewModel> GetTableroViewModelsByUsuario(int idUsuario);
    public Tablero GetTableroById(int idTablero);
    public List<Tablero> GetTableroByUsuario(int idUsuario);
    public void DeleteTablero(int idTablero);
}