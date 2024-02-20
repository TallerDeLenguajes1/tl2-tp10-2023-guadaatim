using Kanban.Models;
using Kanban.ViewModels;
using System.Data.SQLite;
namespace Kanban.Repository;

public class TareaRepository : ITareaRepository
{
    private string _cadenaConexion;

    public TareaRepository(string CadenaDeConexion)
    {
        _cadenaConexion = CadenaDeConexion;
    } 

    public void CreateTarea(int idTablero, Tarea tarea) 
    {
        var queryString = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) 
        VALUES (@idTablero, @nombre, @estado, @descripcion, @color, @idUsuarioAsignado);";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tarea.IdUsuarioAsignado));
            
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Tarea> GetAllTareas()
    {
        var queryString = @"SELECT Tarea.id as idTarea, Tarea.id_tablero as idTablero,
        Tarea.estado as estado, Tarea.descripcion as descripcion, 
        Tarea.color as color, Tarea.id_usuario_asignado as idUsuario, Tarea.nombre as tarea,
        Tablero.nombre as tablero, Usuario.nombre_de_usuario as usuario
        FROM Tarea INNER JOIN Tablero ON Tarea.id_tablero = Tablero.id
        INNER JOIN Usuario ON Tarea.id_usuario_asignado = Usuario.id
        WHERE Tarea.activo = 1;";
        List<Tarea> tareas = new List<Tarea>();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["idTarea"]);
                    tarea.IdTablero = Convert.ToInt32(reader["idTablero"]);
                    tarea.Nombre = reader["tarea"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    if (reader["idUsuario"] == DBNull.Value)
                    {
                        tarea.IdUsuarioAsignado = 0;   
                    } else
                    {
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["idUsuario"]);
                    }
                    tarea.NombreTablero = reader["tablero"].ToString();
                    tarea.NombreUsuario = reader["usuario"].ToString();
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
    }

    public List<Tarea> GetAllTareasByTablero(int idTablero)
    {
        var queryString = @"SELECT Tarea.id as idTarea, Tarea.id_tablero as idTablero,
        Tarea.estado as estado, Tarea.descripcion as descripcion, 
        Tarea.color as color, Tarea.id_usuario_asignado as idUsuario, Tarea.nombre as tarea,
        Tablero.nombre as tablero, Usuario.nombre_de_usuario as usuario
        FROM Tarea INNER JOIN Tablero ON Tarea.id_tablero = Tablero.id
        INNER JOIN Usuario ON Tarea.id_usuario_asignado = Usuario.id
        WHERE Tarea.id_tablero = @idTablero AND Tarea.activo = 1;";
        List<Tarea> tareas = new List<Tarea>();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["idTarea"]);
                    tarea.IdTablero = Convert.ToInt32(reader["idTablero"]);
                    tarea.Nombre = reader["tarea"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    if (reader["idUsuario"] == DBNull.Value)
                    {
                        tarea.IdUsuarioAsignado = 0;   
                    } else
                    {
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["idUsuario"]);
                    }
                    tarea.NombreTablero = reader["tablero"].ToString();
                    tarea.NombreUsuario = reader["usuario"].ToString();
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
    }

    public List<Tarea> GetAllTareasByUsuario(int idUsuario)
    {
        var queryString = @"SELECT Tarea.id as idTarea, Tarea.id_tablero as idTablero,
        Tarea.estado as estado, Tarea.descripcion as descripcion, 
        Tarea.color as color, Tarea.id_usuario_asignado as idUsuario, Tarea.nombre as tarea,
        Tablero.nombre as tablero, Usuario.nombre_de_usuario as usuario
        FROM Tarea INNER JOIN Tablero ON Tarea.id_tablero = Tablero.id
        INNER JOIN Usuario ON Tarea.id_usuario_asignado = Usuario.id
        WHERE Tarea.id_usuario_asignado = @idUsuario AND Tarea.activo = 1;";
        List<Tarea> tareas = new List<Tarea>();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["idTarea"]);
                    tarea.IdTablero = Convert.ToInt32(reader["idTablero"]);
                    tarea.Nombre = reader["tarea"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    if (reader["idUsuario"] == DBNull.Value)
                    {
                        tarea.IdUsuarioAsignado = 0;   
                    } else
                    {
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["idUsuario"]);
                    }
                    tarea.NombreTablero = reader["tablero"].ToString();
                    tarea.NombreUsuario = reader["usuario"].ToString();
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
    }

    public Tarea GetTareaById(int idTarea)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id = @idTarea AND activo = 1;";
        Tarea tarea = new Tarea();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

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
                    if (reader["id_usuario_asignado"] == DBNull.Value)
                    {
                        tarea.IdUsuarioAsignado = 0;   
                    } else
                    {
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    }
                }
            }
            connection.Close();
        }
        return tarea;
    }

    public void UpdateTarea(Tarea tareaModificar)
    {
        var queryString = @"UPDATE Tarea SET nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color, id_usuario_asignado = @idUsuarioAsignado
        WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTarea", tareaModificar.Id));
            command.Parameters.Add(new SQLiteParameter("@nombre", tareaModificar.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tareaModificar.Estado));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tareaModificar.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tareaModificar.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tareaModificar.IdUsuarioAsignado));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Tarea GetTareaByUsuarioAndTablero(int idUsuario, int idTablero)
    {
        var queryString = @"SELECT Tarea.id as idTarea, Tarea.id_tablero as idTablero,
        Tarea.estado as estado, Tarea.descripcion as descripcion, 
        Tarea.color as color, Tarea.id_usuario_asignado as idUsuario, Tarea.nombre as tarea,
        Tablero.nombre as tablero, Usuario.nombre_de_usuario as usuario
        FROM Tarea 
        INNER JOIN Tablero ON Tarea.id_tablero = Tablero.id
        INNER JOIN Usuario ON Tarea.id_usuario_asignado = Usuario.id
        WHERE Tarea.id_usuario_asignado = @idUsuario 
        AND Tarea.id_tablero = @idTablero AND Tarea.activo = 1;";
        Tarea tarea = new Tarea();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["idTarea"]);
                    tarea.IdTablero = Convert.ToInt32(reader["idTablero"]);
                    tarea.Nombre = reader["tarea"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["idUsuario"]);
                    tarea.NombreTablero = reader["tablero"].ToString();
                    tarea.NombreUsuario = reader["usuario"].ToString();
                }
            }
            connection.Close();
        }
        return tarea;
    }

    public void DeleteTarea(int idTarea)
    {
        var queryString = @"UPDATE Tarea SET activo = 0 WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void DeleteTareaByTablero(int idTablero)
    {
        var queryString = @"UPDATE Tarea SET activo = 0 WHERE id_tablero = @idTablero;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void DeleteByUsuario(int idUsuario)
    {
        var queryString = @"UPDATE Tarea SET activo = 0
        WHERE id_tablero = (SELECT id FROM Tablero WHERE id_usuario_propietario= @idUsuario);";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    
    public void UpdateTareaAsignada(int idUsuario)
    {
        var queryString = @"UPDATE Tarea SET id_usuario_asignado = 0 
        WHERE id_usuario_asignado = @idUsuario;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}