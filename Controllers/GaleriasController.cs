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
    public class GaleriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GaleriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Galerias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Galeria>>> GetGalerias()
        {
            return await _context.Galerias.ToListAsync();
        }

        // GET: api/Galerias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Galeria>> GetGaleria(int id)
        {
            var galeria = await _context.Galerias.FindAsync(id);

            if (galeria == null)
            {
                return NotFound();
            }

            return galeria;
        }

        // PUT: api/Galerias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarGaleria(int id, string nombre)
        {
            // Busca la persona por su ID
            var galeriaActual = await _context.Galerias.FindAsync(id);

            if (galeriaActual == null)
            {
                return NotFound(" La Cita no fue encontrado.");
            }

            // Actualiza los campos con los nuevos valores
            galeriaActual.Nombre = nombre;


            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(galeriaActual);
        }

        // POST: api/Galerias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> CrearGaleria(string nombre)
        {
            Galeria galeria = new Galeria()
            {
                Nombre = nombre,
            };
            await _context.Galerias.AddAsync(galeria);
            await _context.SaveChangesAsync();
            return Ok(galeria);
        }

        // DELETE: api/Galerias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGaleria(int id)
        {
            var galeria = await _context.Galerias.FindAsync(id);
            if (galeria == null)
            {
                return NotFound();
            }

            _context.Galerias.Remove(galeria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GaleriaExists(int id)
        {
            return _context.Galerias.Any(e => e.Id == id);
        }
    }
}
