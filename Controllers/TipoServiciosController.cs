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
    public class TipoServiciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoServiciosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoServicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoServicio>>> GetTipoServicios()
        {
            return await _context.TipoServicios.ToListAsync();
        }

        // GET: api/TipoServicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoServicio>> GetTipoServicio(int id)
        {
            var tipoServicio = await _context.TipoServicios.FindAsync(id);

            if (tipoServicio == null)
            {
                return NotFound();
            }

            return tipoServicio;
        }

        // PUT: api/TipoServicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarServicio(int id,  string imagen, string titulo, string descripcion, int precio)
        {
            // Busca la persona por su ID
            var servicioActual = await _context.TipoServicios.FindAsync(id);

            if (servicioActual == null)
            {
                return NotFound(" El servicio no fue encontrado.");
            }

            // Actualiza los campos con los nuevos valores
           
            servicioActual.Imagen = imagen;
            servicioActual.Titulo = titulo;
            servicioActual.Descripcion = descripcion;
            servicioActual.Precio = precio;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(servicioActual);
        }

        // POST: api/TipoServicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> CrearServicio(int precio, string imagen, string titulo, string descripcion, int idusuarioempleado)
        {
            TipoServicio servicio = new TipoServicio()
            {
                Imagen = imagen,
                Titulo = titulo,
                Descripcion = descripcion,
                Precio = precio,
                idusuarioempleado = idusuarioempleado,

            };
            await _context.TipoServicios.AddAsync(servicio);
            await _context.SaveChangesAsync();
            return Ok(servicio);
        }

        // DELETE: api/TipoServicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoServicio(int id)
        {
            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            _context.TipoServicios.Remove(tipoServicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoServicioExists(int id)
        {
            return _context.TipoServicios.Any(e => e.Id == id);
        }
    }
}
