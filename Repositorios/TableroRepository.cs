using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class TableroRepository : ITableroRepository
{
    private string _cadenaConexion;

    public TableroRepository(string CadenaDeConexion)
    {
        _cadenaConexion = CadenaDeConexion;
    }
    
    public void CreateTablero(Tablero tablero)
    {
        var queryString = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
        VALUES (@idUsuario, @nombre, @descripcion);";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();

            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", tablero.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    
    public List<Tablero> GetAllTableros()
    {
        var queryString = @"SELECT Tablero.id as idTablero, Tablero.nombre as tablero, 
        Tablero.descripcion as descripcion, Tablero.id_usuario_propietario as idUsuario,
        Usuario.nombre_de_usuario as usuario
        FROM Tablero 
        INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id
        WHERE Tablero.activo = 1;";
        List<Tablero> tableros = new List<Tablero>();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["idTablero"]);
                    tablero.Nombre = reader["tablero"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["idUsuario"]);
                    tablero.NombreUsuario = reader["usuario"].ToString();
                    tableros.Add(tablero);
                }
            }
            connection.Close();
        }
        if (tableros == null)
        {
            throw new Exception ("La lista de tableros esta vacia");
        }
        return tableros;
    }
    
    public Tablero GetTableroById(int idTablero)
    {
        var queryString = @"SELECT * FROM Tablero WHERE id = @idTablero AND activo = 1;";
        Tablero tablero = new Tablero();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                }
            }
            connection.Close();
        }
        if (tablero == null)
        {
            throw new Exception ("El tablero no existe");
        }
        return tablero;
    }

    public List<Tablero> GetTableroByUsuario(int idUsuario)
    {
        var queryString = @"SELECT Tablero.id as idTablero, Tablero.nombre as tablero, 
        Tablero.descripcion as descripcion, Tablero.id_usuario_propietario as idUsuario,
        Usuario.nombre_de_usuario as usuario
        FROM Tablero INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id
        WHERE Tablero.id_usuario_propietario = @idUsuario AND Tablero.activo = 1;";
        
        List<Tablero> tableros = new List<Tablero>();

        using(SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["idTablero"]);
                    tablero.Nombre = reader["tablero"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["idUsuario"]);
                    tablero.NombreUsuario = reader["usuario"].ToString();
                    tableros.Add(tablero);
                }
            }
            connection.Close();
        }
        if (tableros == null)
        {
            throw new Exception ("La lista de tableros esta vacia");
        }
        return tableros;
    }
    
    public void UpdateTablero(Tablero tableroModificar)
    {
        var queryString = @"UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion, id_usuario_propietario = @idUsuario
        WHERE id = @idTablero;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            command.Parameters.Add(new SQLiteParameter("@idTablero", tableroModificar.Id));
            command.Parameters.Add(new SQLiteParameter("@nombre", tableroModificar.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tableroModificar.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@idUsuario", tableroModificar.IdUsuarioPropietario));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Tablero> GetTablerosByTarea(int idUsuario)
    {
        var queryString = @"SELECT DISTINCT Tablero.id as idTablero, 
        Tablero.id_usuario_propietario as idUsuario,
        Tablero.nombre as tablero, Tablero.descripcion as descripcion,
        Usuario.nombre_de_usuario as usuario
        FROM Tablero INNER JOIN Tarea ON Tablero.id = Tarea.id_tablero
        INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id
        WHERE Tarea.id_usuario_asignado = @idUsuario AND Tablero.activo = 1;";
        List<Tablero> tableros = new List<Tablero>();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["idTablero"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["idUsuario"]);
                    tablero.Nombre = reader["tablero"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.NombreUsuario = reader["usuario"].ToString();
                    tableros.Add(tablero);
                }
            }
            connection.Close();
        }
        if (tableros == null)
        {
            throw new Exception ("La lista de tableros esta vacia");
        }
        return tableros;
    }

    public bool PerteneceTablero(int idUsuario, int idTablero)
    {
        var queryString = @"SELECT COUNT (id) as pertenece FROM Tablero 
        WHERE id_usuario_propietario = @idUsuario AND id = @idTablero AND activo = 1;";
        bool pertenece = false;

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
                    if(Convert.ToInt32(reader["pertenece"]) == 1)
                    {
                        pertenece = true;
                    }
                }
            }
            connection.Close();
        } 
        return pertenece;
    }

    public void DeleteTablero(int idTablero)
    {
        var queryString = @"UPDATE Tablero SET activo = 0 WHERE id = @idTablero;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void DeleteTableroByUsuario(int idUsuario)
    {
        var queryString = @"UPDATE Tablero SET activo = 0 WHERE id_usuario_propietario = @idUsuario;";

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