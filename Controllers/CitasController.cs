using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peluqueria.Context;
using Peluqueria.Models;

namespace Peluqueria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }




		[HttpGet]
		public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
		{
			// Filtrar empleados con estado "Activo"
			var citasActivos = await _context.Citas
				.Where(e => e.Estado == "Activo")
				.ToListAsync();

			// Retornar la lista de empleados activos
			return citasActivos;
		}


		// GET: api/Citas/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);

            if (cita == null)
            {
                return NotFound();
            }

            return cita;
        }

        // PUT: api/Citas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarCita(int id, string fecha, int idusuariocliente, int idhora, int idtiposervicio)
        {
            // Busca la persona por su ID
            var citaActual = await _context.Citas.FindAsync(id);

            if (citaActual == null)
            {
                return NotFound(" La Cita no fue encontrado.");
            }

            // Actualiza los campos con los nuevos valores
            citaActual.Fecha = fecha;
            citaActual.idusuariocliente = idusuariocliente;
            citaActual.idhora = idhora;
            citaActual.idtiposervicio = idtiposervicio;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(citaActual);
        }

        // POST: api/Citas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> CrearCita( string fecha, int total,int idhora, int idusuariocliente, int idtiposervicio)
        {
            Cita cita = new Cita()
            {
                Fecha = fecha,                
                Total = total,
                Estado = "Activo",
                idusuariocliente=idusuariocliente,
                idhora = idhora,
                idtiposervicio = idtiposervicio
            };
            await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();
            return Ok(cita);
        }

        // DELETE: api/Citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var citaCancelada = await _context.Citas.FindAsync(id);

            if (citaCancelada == null)
            {
                return NotFound("La cita no fue encontrado.");
            }

            // Cambiar el estado a "Inactivo" en lugar de eliminar
            citaCancelada.Estado = "Inactivo";

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { message = "La cita ha sido Cancelada." });
        }
    }
}
