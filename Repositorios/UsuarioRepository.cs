using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private string cadenaConexion;
    
    public UsuarioRepository(string CadenaDeConexion)
    {
        cadenaConexion = CadenaDeConexion;
    }

    public void CreateUsuario(Usuario usuarioNuevo)
    {
        var queryString = @"INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES(@nombre, @contrasenia, @rol);";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@nombre", usuarioNuevo.NombreDeUsuario));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", usuarioNuevo.Contrasenia));
            command.Parameters.Add(new SQLiteParameter("@rol", usuarioNuevo.Rol));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public void UpdateUsuario(int idUsuario, Usuario usuarioModificar)
    {
        var queryString = @"UPDATE Usuario SET nombre_de_usuario = @nombre, contrasenia = @contrasenia, rol = @rol WHERE id = @idUsuario;";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@nombre", usuarioModificar.NombreDeUsuario));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", usuarioModificar.Contrasenia));
            command.Parameters.Add(new SQLiteParameter("@rol", usuarioModificar.Rol));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public List<Usuario> GetAllUsuarios()
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
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.Rol = (Rol)Convert.ToInt32(reader["rol"]);                
                    usuarios.Add(usuario);
                }
            }
            connection.Close();
        }
        if (usuarios == null)
        {
            throw new Exception("La lista de usuarios esta vacia");
        } else
        {
            return usuarios;
        }
    }
    public Usuario GetUsuarioById(int idUsuario)
    {
        var queryString = @"SELECT * FROM Usuario WHERE id = @idUsuario;";
        Usuario usuario = null;

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.Rol = (Rol)Convert.ToInt32(reader["rol"]);
                }
            }
        connection.Close();
        }

        if(usuario == null)
        {
            throw new Exception("El usuario no existe");
        } else
        {
            return usuario;
        }
    }
    public void DeleteUsuario(int idUsuario)
    {
        var queryString = @"DELETE FROM Usuario WHERE id = @idUsuario;";

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