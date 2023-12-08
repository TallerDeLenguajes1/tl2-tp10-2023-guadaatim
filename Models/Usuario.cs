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

    public Usuario(string nombreDeUsuario, string contrasenia, Rol rol)
    {
        this.nombreDeUsuario = nombreDeUsuario;
        this.contrasenia = contrasenia;
        this.rol = rol;
    }

    public int Id { get => id; set => id = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public Rol Rol { get => rol; set => rol = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
}