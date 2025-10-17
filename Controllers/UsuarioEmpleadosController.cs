using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Peluqueria.Context;
using Peluqueria.Models;

namespace Peluqueria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioEmpleadosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IConfiguration _configuration;
        public UsuarioEmpleadosController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // GET: api/UsuarioEmpleados
        [HttpGet("ListarEmpleados")]
        public async Task<ActionResult<IEnumerable<UsuarioEmpleado>>> GetUsuariosEmpleados()
        {
            var usuariosRoles = await _context.UsuariosEmpleados
                .Join(
                    _context.Roles, // Tabla de roles
                    usuario => usuario.idrol, // Clave foránea en UsuariosEmpleados
                    rol => rol.Id, // Clave primaria en Roles
                    (usuario, rol) => new
                    {
                        NombreUsuario = usuario.NombreUsuario,
                        Correo = usuario.Correo,
                        Password = usuario.Password,
                        Telefono = usuario.Telefono,
                        Rol = rol.Descripcion // Nombre del rol
                    }
                )
                .ToListAsync();

            return Ok(usuariosRoles);
        }
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UsuarioEmpleado>>> GetUsuariosEmpleado()
		{
			// Filtrar empleados con estado "Activo"
			var empleadosActivos = await _context.UsuariosEmpleados
				.Where(e => e.Estado == "Activo")
				.ToListAsync();

			// Retornar la lista de empleados activos
			return empleadosActivos;
		}


		// GET: api/UsuarioEmpleados/5
		[HttpGet("{id}")]
        public async Task<ActionResult<UsuarioEmpleado>> GetUsuarioEmpleado(int id)
        {
            var usuarioEmpleado = await _context.UsuariosEmpleados.FindAsync(id);

            if (usuarioEmpleado == null)
            {
                return NotFound();
            }

            return usuarioEmpleado;
        }

        //Login

        [HttpPost("LoginEmpleado")]
        public async Task<IActionResult> Login(string correo, string password)
        {
            var usuario = await _context.UsuariosEmpleados.SingleOrDefaultAsync(u => u.Correo == correo && u.Password == password);
            if (usuario == null)
                return BadRequest(new { message = "Credenciales invalidas" });

            string jwtToken = GenerarToken(usuario);
            // Devolvemos el token, el ID y el nombre del usuario
            return Ok(new { token = jwtToken, id = usuario.Id, nombre = usuario.NombreUsuario });
        }

        private string GenerarToken(UsuarioEmpleado usuario)
        {
            var Myclaims = new[]
            {
         new Claim(ClaimTypes.Name, usuario.Correo)
     };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                claims: Myclaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credenciales
            );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }


        // PUT: api/UsuarioEmpleados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarUsuarioEmpleado(int id, string nombreusuario, int telefono, string correo, string password, string estado)
        {
            // Busca la persona por su ID
            var empleadoActual = await _context.UsuariosEmpleados.FindAsync(id);

            if (empleadoActual == null)
            {
                return NotFound("El empleado no fue encontrado.");
            }

            // Actualiza los campos con los nuevos valores
            empleadoActual.NombreUsuario = nombreusuario;
            empleadoActual.Telefono = telefono;
            empleadoActual.Correo = correo;
            empleadoActual.Password = password;
            empleadoActual.Estado = estado;
            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(empleadoActual);
        }

        // POST: api/UsuarioEmpleados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> CrearPublicacion(string nombreusaurio, int telefono, string correo, string password, int idrol)
        {
            UsuarioEmpleado empleado = new UsuarioEmpleado()
            {
                NombreUsuario = nombreusaurio,
                Telefono = telefono,
                Correo = correo,
                Password = password,
                Estado = "Activo",
                idrol = idrol,
            };
            await _context.UsuariosEmpleados.AddAsync(empleado);
            await _context.SaveChangesAsync();
            return Ok(empleado);
        }

        // DELETE: api/UsuarioEmpleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioEmpleado(int id)
        {
            var usuarioEmpleado = await _context.UsuariosEmpleados.FindAsync(id);

            if (usuarioEmpleado == null)
            {
                return NotFound("El usuario no fue encontrado.");
            }

            // Cambiar el estado a "Inactivo" en lugar de eliminar
            usuarioEmpleado.Estado = "Inactivo";

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { message = "El usuario ha sido desactivado." });
        }
    }
}
