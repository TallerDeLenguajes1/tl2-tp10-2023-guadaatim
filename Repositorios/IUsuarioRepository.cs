using Kanban.Models;

namespace Kanban.Repository;

public interface IUsuarioRepository
{ 
    public void CreateUsuario(Usuario usuarioNuevo);
    public bool ExisteUsuario(string nombreUsuario);
    public void UpdateUsuario(Usuario usuarioModificar);
    public List<Usuario> GetAllUsuarios();
    public Usuario GetUsuarioByNombre(string nombreUsuario);
    public Usuario GetUsuarioById(int idUsuario);
    public void DeleteUsuario(int idUsuario);
}
    