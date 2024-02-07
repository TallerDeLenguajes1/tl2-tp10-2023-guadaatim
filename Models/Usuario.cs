using Kanban.ViewModels;

namespace Kanban.Models;

public enum Rol
{
    Administrador = 0,
    Operador = 1
}

public class Usuario
{
    private int id;
    private string nombreDeUsuario;
    private string contrasenia;
    private Rol rol;

    public Usuario()
    {
    }

    public Usuario(CrearUsuarioViewModel usuarioVM)
    {
        this.nombreDeUsuario = usuarioVM.NombreDeUsuario;
        this.contrasenia = usuarioVM.Contrasenia;
        this.rol = usuarioVM.Rol;
    }

    public Usuario(ModificarUsuarioViewModel usuarioVM)
    {
        this.id = usuarioVM.Id;
        this.nombreDeUsuario = usuarioVM.NombreDeUsuario;
        this.contrasenia = usuarioVM.Contrasenia;
        this.rol = usuarioVM.Rol;
    }

    public int Id { get => id; set => id = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public Rol Rol { get => rol; set => rol = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
}