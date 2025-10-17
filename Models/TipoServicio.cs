using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Peluqueria.Models
{
    public class TipoServicio
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public ICollection<Cita> Cita { get; set; }

        [ForeignKey("UsuarioEmpleado")]
        public int idusuarioempleado { get; set; }
        [JsonIgnore]
        public UsuarioEmpleado UsuarioEmpleado { get; set; }
    }
}
