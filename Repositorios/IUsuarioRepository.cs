using Kanban.Models;

namespace Kanban.Repository;

public interface IUsuarioRepository
{ 
    public void CreateUsuario(Usuario usuarioNuevo);
    public void UpdateUsuario(int idUsuario, Usuario usuarioModificar);
    public List<Usuario> GetAllUsuarios();
    public Usuario GetUsuarioById(int idUsuario);
    public void DeleteUsuario(int idUsuario);
}
    