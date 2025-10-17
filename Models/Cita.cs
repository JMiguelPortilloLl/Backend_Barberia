using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Peluqueria.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }

        [ForeignKey("UsuarioCliente")]
        public int idusuariocliente { get; set; }
        [JsonIgnore]
        public UsuarioCliente UsuarioCliente { get; set; }

        [ForeignKey("Hora")]
        public int idhora { get; set; }
        [JsonIgnore]
        public Hora Hora { get; set; }

        [ForeignKey("TipoServicio")]
        public int idtiposervicio { get; set; }
        [JsonIgnore]
        public TipoServicio TipoServicio { get; set; }
    }
}
