using Kanban.Models;
using System.Data.SQLite;
namespace Kanban.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private string _cadenaConexion;
    
    public UsuarioRepository(string CadenaDeConexion)
    {
        _cadenaConexion = CadenaDeConexion;
    }

    public void CreateUsuario(Usuario usuarioNuevo)
    {
        var queryString = @"INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES(@nombre, @contrasenia, @rol);";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
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

    public bool ExisteUsuario(string nombreUsuario)
    {
        var queryString = @"SELECT COUNT(id) as existe FROM Usuario WHERE nombre_de_usuario = @nombreUsuario;";
        bool existe = false;

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombreUsuario));

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int contador = Convert.ToInt32(reader["existe"]);

                    if (contador == 1)
                    {
                        existe = true;
                    }
                }
            }
            connection.Close();
        }
        return existe;
    }

    public void UpdateUsuario(Usuario usuarioModificar)
    {
        var queryString = @"UPDATE Usuario SET nombre_de_usuario = @nombre, contrasenia = @contrasenia, rol = @rol WHERE id = @idUsuario;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            command.Parameters.Add(new SQLiteParameter("@idUsuario", usuarioModificar.Id));
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
        
        using(SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
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
        return usuarios;
    }
    public Usuario GetUsuarioByNombre(string nombreUsuario)
    {
        var queryString = @"SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUsuario;";
        Usuario usuario = new Usuario();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombreUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.Rol = (Rol)Convert.ToInt32(reader["rol"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }

    public Usuario GetUsuarioById(int idUsuario)
    {
        var queryString = @"SELECT * FROM Usuario WHERE id = @idUsuario;";
        Usuario usuario = new Usuario();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
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
        return usuario;
    }

    public void DeleteUsuario(int idUsuario)
    {
        var queryString = @"DELETE FROM Usuario WHERE id = @idUsuario;";

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public bool ExisteUsuarioLogin(Usuario usuario)
    {
        var queryString = @"SELECT COUNT(id) as existe FROM Usuario 
        WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contrasenia;";
        bool existe = false;

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", usuario.NombreDeUsuario));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));
            
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["existe"]) == 1)
                    {
                        existe = true;
                    }
                }
            }
            connection.Close();
        }
        return existe;
    }

    public List<Usuario> GetAllUsuariosExcept(int idUsuario)
    {
        var queryString = @"SELECT * FROM Usuario WHERE id IS NOT @idUsuario;";
        List<Usuario> usuarios = new List<Usuario>();

        using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.Rol = (Rol) Convert.ToInt32(reader["rol"]);
                    usuarios.Add(usuario);
                }
            }
            connection.Close();
        }
        return usuarios;
    }
}