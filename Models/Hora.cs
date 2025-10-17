namespace Peluqueria.Models
{
    public class Hora
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ICollection<Cita> Cita { get; set; }
    }
}
