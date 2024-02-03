using Kanban.Models;
using Kanban.ViewModels;
using System.Data.SQLite;
namespace Kanban.Repository;

public class TareaRepository : ITareaRepository
{
    private string cadenaConexion;

    public TareaRepository(string CadenaDeConexion)
    {
        cadenaConexion = CadenaDeConexion;
    } 

    public void CreateTarea(int idTablero, Tarea tarea) 
    {
        var queryString = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) 
        VALUES (@idTablero, @nombre, @estado, @descripcion, @color, @idUsuarioAsignado);";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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

    public void AsignarUsuario(int idUsuario, int idTarea)
    {
        var queryString = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Tarea> GetAllTareas()
    {
        var queryString = @"SELECT * FROM Tarea;";
        List<Tarea> tareas = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tareas == null)
                    {
                        tareas = new List<Tarea>();
                    }
                    Tarea tarea = new Tarea();
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
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }

        if(tareas == null)
        {
            throw new Exception("La lista de tareas esta vacia");
        } else
        {
            return tareas;
        }
    }

    public List<Tarea> GetAllTareasByTablero(int idTablero)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero;";
        List<Tarea> tareas = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tareas == null)
                    {
                        tareas = new List<Tarea>();
                    }
                    Tarea tarea = new Tarea();
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
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
    
        if (tareas == null)
        {
            throw new Exception("El tablero todavia no tiene tareas asignadas");
        } else
        {
            return tareas;
        }
    }

    public List<Tarea> GetAllTareasByUsuario(int idUsuario)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @idUsuario;";
        List<Tarea> tareas = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (tareas == null)
                    {
                        tareas = new List<Tarea>();                        
                    }
                    Tarea tarea = new Tarea();
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
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }

        if(tareas == null)
        {
            throw new Exception("El usuario todavia no tiene tareas asignadas");
        } else
        {
            return tareas;
        }
    }

    public Tarea GetTareaById(int idTarea)
    {
        var queryString = @"SELECT * FROM Tarea WHERE id = @idTarea;";
        Tarea tarea = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tarea = new Tarea();
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

        if(tarea == null)
        {
            throw new Exception("La tarea no existe");
        } else
        {
            return tarea;
        }
    }

    public void UpdateTarea(Tarea tareaModificar)
    {
        var queryString = @"UPDATE Tarea SET nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color, id_usuario_asignado = @idUsuarioAsignado
        WHERE id = @idTarea;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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

    public List<TareaViewModel> GetTareasViewModel()
    {
        var queryString = @"SELECT Tarea.id as idTarea, Tablero.id as idTablero,
        Tarea.estado as estado, Tarea.descripcion as descripcion, 
        Tarea.color as color, Tarea.id_usuario_asignado as idUsuario, 
        Tarea.nombre as tarea, Tablero.nombre as tablero, Usuario.nombre_de_usuario as usuario
        FROM Tarea 
        INNER JOIN Tablero ON Tarea.id_tablero = Tablero.id
        INNER JOIN Usuario ON Tarea.id_usuario_asignado = Usuario.id;";
        List<TareaViewModel> tareas = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tareas == null)
                    {
                        tareas = new List<TareaViewModel>();
                    }
                    TareaViewModel tarea = new TareaViewModel();
                    tarea.Id = Convert.ToInt32(reader["idTarea"]);
                    tarea.IdTablero = Convert.ToInt32(reader["idTablero"]);
                    tarea.Nombre = reader["tarea"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["idUsuario"]);
                    tarea.NombreTablero = reader["tablero"].ToString();
                    tarea.NombreUsuario = reader["usuario"].ToString();
                    tareas.Add(tarea);
                }
            } 
            connection.Close();           
        }
        if(tareas != null)
        {
            return tareas;
        } else
        {
            throw new Exception("La lista de tareas esta vacia");
        }
    }

    public List<TareaViewModel> GetTareaViewModelByUsuario(int idTarea)
    {
        var queryString = @"SELECT Tarea.id as idTarea, Tablero.id as idTablero,
        Tarea.estado as estado, Tarea.descripcion as descripcion, 
        Tarea.color as color, Tarea.id_usuario_asignado as idUsuario, 
        Tarea.nombre as tarea, Tablero.nombre as tablero, Usuario.nombre_de_usuario as usuario
        FROM Tarea 
        INNER JOIN Tablero ON Tarea.id_tablero = Tablero.id
        INNER JOIN Usuario ON Tarea.id_usuario_asignado = Usuario.id
        WHERE @idTarea = Usuario.id;";
        List<TareaViewModel> tareas = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tareas == null)
                    {
                        tareas = new List<TareaViewModel>();
                    }
                    TareaViewModel tarea = new TareaViewModel();
                    tarea.Id = Convert.ToInt32(reader["idTarea"]);
                    tarea.IdTablero = Convert.ToInt32(reader["idTablero"]);
                    tarea.Nombre = reader["tarea"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["idUsuario"]);
                    tarea.NombreTablero = reader["tablero"].ToString();
                    tarea.NombreUsuario = reader["usuario"].ToString();
                    tareas.Add(tarea);
                }
            } 
            connection.Close();           
        }
        if(tareas != null)
        {
            return tareas;
        } else
        {
            throw new Exception("La lista de tareas esta vacia");
        }
    }
}