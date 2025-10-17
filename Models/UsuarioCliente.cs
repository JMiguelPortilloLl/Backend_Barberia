namespace Peluqueria.Models
{
    public class UsuarioCliente
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public int CI { get; set; }
        public int Telefono { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Estado { get; set; }
        public ICollection<Cita> Cita { get; set; }
    }
}
