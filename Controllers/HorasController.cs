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
    public class HorasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HorasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Horas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hora>>> GetHoras()
        {
            // Filtrar las horas con estado "Activo"
            return await _context.Horas
                .Where(h => h.Estado == "Activo")
                .ToListAsync();
        }

        // GET: api/Horas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hora>> GetHora(int id)
        {
            var hora = await _context.Horas.FindAsync(id);

            if (hora == null)
            {
                return NotFound();
            }

            return hora;
        }

        // PUT: api/Horas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarHora(int id, string descripcion, string estado)
        {
            // Busca la persona por su ID
            var horaActual = await _context.Horas.FindAsync(id);

            if (horaActual == null)
            {
                return NotFound(" La hora no fue encontrado.");
            }

            // Actualiza los campos con los nuevos valores
            horaActual.Descripcion = descripcion;


            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(horaActual);
        }


        // POST: api/Horas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> CrearHora(string descripcion)
        {
            Hora hora = new Hora()
            {
                Descripcion = descripcion,
                Estado = "Activo"
            };
            await _context.Horas.AddAsync(hora);
            await _context.SaveChangesAsync();
            return Ok(hora);
        }

        // DELETE: api/Citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHora(int id)
        {
            var horaCancelada = await _context.Horas.FindAsync(id);

            if (horaCancelada == null)
            {
                return NotFound("La hora no fue encontrada.");
            }

            // Cambiar el estado a "Inactivo" en lugar de eliminar
            horaCancelada.Estado = "Inactivo";

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { message = "La hora ha sido Cancelada." });
        }
    }
}
