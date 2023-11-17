using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class TareaRepository : ITareaRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

    public void CreateTarea(int idTablero, Tarea tarea) //devuelve tablero ???
    {
        var queryString = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color) 
        VALUES (@idTablero, @nombre, @estado, @descripcion, @color);";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

     public void AsignarUsuario(int idUsuario, int idTarea)
    {
        var queryString = @"UPDATE Tarea SET id_usuario_propietario = @idUsuario WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Tarea> GetAllTareas()
    {
        var queryString = @"SELECT * FROM Tarea;";
        List<Tarea> tareas = new List<Tarea>();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
    }

    public List<Tarea> GetAllTareasByTablero(int idTablero)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero;";
        List<Tarea> tareas = new List<Tarea>();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
    }

    public List<Tarea> GetAllTareasByUsuario(int idUsuario)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id_usuario_propietario = @idUsuario;";
        List<Tarea> tareas = new List<Tarea>();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
    }

    public Tarea GetTareaById(int idTarea)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id = @idTarea;";
        Tarea tarea = new Tarea();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_propietario"]);
                }
            }
            connection.Close();
        }
        return tarea;
    }

    public void UpdateTarea(int idTarea, Tarea tareaModificar)
    {
        var queryString = @"UPDATE Tarea SET nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color
        WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            command.Parameters.Add(new SQLiteParameter("@nombre", tareaModificar.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tareaModificar.Estado));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tareaModificar.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tareaModificar.Color));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    
    public void DeleteTarea(int idTarea)
    {
        var queryString = @"DELETE FROM Tarea WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}