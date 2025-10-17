using Microsoft.EntityFrameworkCore;
using Peluqueria.Models;

namespace Peluqueria.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) 
        { 
        }
        public DbSet<UsuarioCliente> UsuariosClientes { get; set; }
        public DbSet<UsuarioEmpleado> UsuariosEmpleados { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<Hora> Horas { get; set; }
        public DbSet<Galeria> Galerias { get; set; } = default!;
    }
}
