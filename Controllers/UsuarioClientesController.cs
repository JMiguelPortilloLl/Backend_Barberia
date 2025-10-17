using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Peluqueria.Context;
using Peluqueria.Models;

namespace Peluqueria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioClientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IConfiguration _configuration;
        public UsuarioClientesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }




		[HttpGet]
		public async Task<ActionResult<IEnumerable<UsuarioCliente>>> GetUsuariosClientes()
		{
			// Filtrar empleados con estado "Activo"
			var clienteActivos = await _context.UsuariosClientes
				.Where(e => e.Estado == "Activo")
				.ToListAsync();

			// Retornar la lista de empleados activos
			return clienteActivos;
		}


		// GET: api/UsuarioClientes/5
		[HttpGet("{id}")]
        public async Task<ActionResult<UsuarioCliente>> GetUsuarioCliente(int id)
        {
            var usuarioCliente = await _context.UsuariosClientes.FindAsync(id);

            if (usuarioCliente == null)
            {
                return NotFound();
            }

            return usuarioCliente;
        }

        // GET: api/Usuarios/5
        [HttpGet]
        [Route("CitasUsuario")]

        public async Task<ActionResult<IEnumerable<Cita>>> CitasUsuario(int id_persona)
        {
            var usuario = await _context.UsuariosClientes
                .Include(p => p.Cita)
                .FirstOrDefaultAsync(pu => pu.Id == id_persona);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario.Cita);
        }
        //Login

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string correo, string password)
        {
            var usuario = await _context.UsuariosClientes.SingleOrDefaultAsync(u => u.Correo == correo && u.Password == password);
            if (usuario == null)
                return BadRequest(new { message = "Credenciales invalidas" });

            string jwtToken = GenerarToken(usuario);
            // Devolvemos el token, el ID y el nombre del usuario
            return Ok(new { token = jwtToken, id = usuario.Id, nombre = usuario.NombreUsuario });
        }

        private string GenerarToken(UsuarioCliente usuario)
        {
            var Myclaims = new[]
            {
         new Claim(ClaimTypes.Name, usuario.Correo)
     };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                claims: Myclaims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credenciales
            );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
        // PUT: api/UsuarioClientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarUsuarioCliente(int id, string nombreusuario, string correo, string password, int ci, int telefono)
        {
            // Busca la persona por su ID
            var clienteActual = await _context.UsuariosClientes.FindAsync(id);

            if (clienteActual == null)
            {
                return NotFound("La persona no fue encontrada.");
            }

            // Actualiza los campos con los nuevos valores
            clienteActual.NombreUsuario = nombreusuario;
            clienteActual.Telefono = telefono;
            clienteActual.CI = ci;
            clienteActual.Correo = correo;
            clienteActual.Password = password;
            

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(clienteActual);
        }

        // POST: api/UsuarioClientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CrearCliente(string nombreusuario, string correo, string password, int ci, int telefono)
        {
            UsuarioCliente cliente = new UsuarioCliente()
            {
                NombreUsuario = nombreusuario,
                CI = ci,
                Telefono = telefono,
                Correo = correo,
                Password = password,
                Estado = "Activo"

            };
            await _context.UsuariosClientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return Ok(cliente);
        }

        // DELETE: api/UsuarioClientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioCliente(int id)
        {
            var usuarioCliente = await _context.UsuariosClientes.FindAsync(id);

            if (usuarioCliente == null)
            {
                return NotFound("El usuario no fue encontrado.");
            }

            // Cambiar el estado a "Inactivo" en lugar de eliminar
            usuarioCliente.Estado = "Inactivo";

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { message = "El usuario ha sido desactivado." });
        }
    }
}
