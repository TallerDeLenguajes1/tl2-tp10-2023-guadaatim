using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class TableroRepository : ITableroRepository
{
    private string cadenaConexion;

    public TableroRepository(string CadenaDeConexion)
    {
        cadenaConexion = CadenaDeConexion;
    }
    
    public void CreateTablero(Tablero tablero)
    {
        var queryString = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
        VALUES (@idUsuario, @nombre, @descripcion);";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
        var queryString = @"SELECT * FROM Tablero;";
        List<Tablero> tableros = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (tableros == null)
                    {
                        tableros = new List<Tablero>();
                    }
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tableros.Add(tablero);
                }
            }
            connection.Close();
        }

        if(tableros == null)
        {
            throw new Exception("La lista de tableros esta vacia");
        } else
        {
            return tableros;
        }
    }
    
    public Tablero GetTableroById(int idTablero)
    {
        var queryString = @"SELECT * FROM Tablero WHERE id = @idTablero;";
        Tablero tablero = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                }
            }
            connection.Close();
        }

        if(tablero == null)
        {
            throw new Exception("El tablero no existe");
        } else
        {
            return tablero;
        }
    }

    public List<Tablero> GetTableroByUsuario(int idUsuario)
    {
        var queryString = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario;";
        List<Tablero> tableros = null;

        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tableros == null)
                    {
                        tableros = new List<Tablero>();
                    }
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tableros.Add(tablero);
                }
            }
            connection.Close();
        }

        if (tableros == null)
        {
            throw new Exception("El usuario todavia no tiene tableros asignados");
        } else
        {
            return tableros;
        }
    }
    
    public void UpdateTablero(Tablero tableroModificar)
    {
        var queryString = @"UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion, id_usuario_propietario = @idUsuario
        WHERE id = @idTablero;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
    
    public void DeleteTablero(int idTablero)
    {
        var queryString = @"DELETE FROM Tablero WHERE id = @idTablero;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<TableroViewModel> GetTablerosViewModel()
    {
        var queryString = @"SELECT Tablero.id as idTablero, Tablero.id_usuario_propietario as idUsuario,
        Tablero.nombre as tablero, Tablero.descripcion as descripcion,
        Usuario.nombre_de_usuario as usuario
        FROM Tablero 
        INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id;";
        List<TableroViewModel> tableros = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tableros == null)
                    {
                        tableros = new List<TableroViewModel>();
                    }
                    TableroViewModel tablero = new TableroViewModel();
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

        if(tableros == null)
        {
            throw new Exception("La lista de tableros esta vacia");
        } else
        {
            return tableros;
        }
    }

    public List<TableroViewModel> GetTableroViewModelsByUsuario(int idUsuario)
    {
        var queryString = @"SELECT Tablero.id as idTablero, Tablero.id_usuario_propietario as idUsuario,
        Tablero.nombre as tablero, Tablero.descripcion as descripcion,
        Usuario.nombre_de_usuario as usuario
        FROM Tablero 
        INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id
        WHERE Usuario.id = @idUsuario;";
        List<TableroViewModel> tableros = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(tableros == null)
                    {
                        tableros = new List<TableroViewModel>();
                    }
                    TableroViewModel tablero = new TableroViewModel();
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

        if(tableros == null)
        {
            throw new Exception("La lista de tableros esta vacia");
        } else
        {
            return tableros;
        }
    }
}