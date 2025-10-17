namespace Peluqueria.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public ICollection<UsuarioEmpleado> UsuarioEmpleado { get; set; }
    }
}
