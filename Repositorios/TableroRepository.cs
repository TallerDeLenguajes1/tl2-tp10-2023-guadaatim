using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class TableroRepository : ITableroRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";
    
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
        List<Tablero> tableros = new List<Tablero>();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
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
        return tableros;
    }
    
    public Tablero GetTableroById(int idTablero)
    {
        var queryString = @"SELECT * FROM Tablero WHERE id = @idTablero;";
        Tablero tablero = new Tablero();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
        return tablero;
    }

    public List<Tablero> GetTableroByUsuario(int idUsuario)
    {
        var queryString = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario;";
        List<Tablero> tableros = new List<Tablero>();

        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
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
        return tableros;
    }
    
    public void UpdateTablero(int idTablero, Tablero tableroModificar)
    {
        var queryString = @"UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion, id_usuario_propietario = @idUsuario
        WHERE id = @idTablero;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            command.Parameters.Add(new SQLiteParameter("idTablero", idTablero));
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
}