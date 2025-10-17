using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Peluqueria.Models
{
    public class UsuarioEmpleado
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public int Telefono { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Estado {  get; set; }

        public ICollection<TipoServicio> TipoServicio { get; set; }

        [ForeignKey("Rol")]
        public int idrol { get; set; }
        [JsonIgnore]
        public Rol Rol { get; set; }
    }
}
