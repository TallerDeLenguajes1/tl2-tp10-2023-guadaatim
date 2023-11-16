using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db:Cache=Shared";
    public void CreateUsuario(Usuario usuarioNuevo)
    {
        var queryString = @"INSERT INTO Usuario (nombre_de_usuario) VALUES(@nombre);";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@nombre", usuarioNuevo.NombreDeUsuario));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public void UpdateUsuario(int idUsuario, Usuario usuarioModificar)
    {
        var queryString = @"UPDATE Usuario SET nombre_de_usuario = (@nombre) WHERE id = (@idUsuario);";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@nombre", usuarioModificar.NombreDeUsuario));
            command.Parameters.Add(new SQLiteParameter("@id", idUsuario));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public List<Usuario> GetAllUsuario()
    {
        var queryString = @"SELECT * FROM Usuario;";
        List<Usuario> usuarios = new List<Usuario>();
        
        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();                
                    usuarios.Add(usuario);
                }
            }
            connection.Close();
        }
        return usuarios;
    }
    public Usuario GetUsuarioById(int idUsuario)
    {
        var queryString = @"SELECT * FROM Usuario WHERE id = @idUsuario;";
        Usuario usuario = new Usuario();

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                }
            }
            
        connection.Close();
        }
        return usuario;
    }
    public void EliminarUsuario(int idUsuario)
    {
        var queryString = @"DELETE * FROM Usuario WHERE id = @idUsuario;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}