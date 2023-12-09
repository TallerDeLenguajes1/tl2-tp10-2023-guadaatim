using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ListarUsuariosViewModel
{
    private List<UsuarioViewModel> usuariosVM;
    
    public ListarUsuariosViewModel(List<Usuario> usuarios)
    {
        usuariosVM = new List<UsuarioViewModel>();

        foreach (var usuario in usuarios)
        {
            UsuarioViewModel usuarioNuevo = new UsuarioViewModel(usuario);
            usuariosVM.Add(usuarioNuevo);
        }
    }

    public List<UsuarioViewModel> UsuariosVM { get => usuariosVM; set => usuariosVM = value; }
}