using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica4.Data;
using practica4.Models;
using practica4.Seguridad;

namespace practica4.Controllers
{
    [ApiController]
    [Apikeyattribute]
    [Route("api/[controller]")]
    public class libroController : Controller
    {
        private readonly AppDbContext _context;

        public libroController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("getlibros")]
        public async Task<ActionResult> GetLibros()
        {
            var libros = await _context.Libro.ToListAsync();
            return Ok(libros);
        }
        [HttpGet("getlibros/{id}")]
        public async Task<ActionResult> GetLibrosporid(int id)
        {
            var libro = await _context.Libro.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }
        [HttpGet("librospaginado")]
        public async Task<ActionResult> GetLibroPaginado(int pag=1, int sizepag=5,string? busqueda = null, string? orden="Precio", string direccion = "ascendente")
        {
            if (pag <= 0 || sizepag <= 0)
            {
                return BadRequest(new { mensaje = "Debes introducir un numero mayor o igual a 1" });
            }
            var consulta = _context.Libro.AsNoTracking().AsQueryable();
            if(!string.IsNullOrEmpty(busqueda))
            {
                consulta = consulta.Where(l => l.Titulo.Contains(busqueda));
            }
            consulta = direccion.ToLower() == "descendiente"
                ? consulta.OrderByDescending(l => EF.Property<Object>(l, orden))
                : consulta.OrderBy(l => EF.Property<Object>(l, orden));
            var libros = await consulta
                .Skip((pag - 1) * sizepag)
                .Take(sizepag)
                .ToListAsync();
            var resultado = new
            {
                pag,
                sizepag,
                datos = libros
            };
            return Ok(resultado);
        }
        [HttpPost("crearlibro")]
        public async Task<ActionResult> CrearLibro([FromBody] Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Libro.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibrosporid), new { id = libro.Id }, libro);
        }

        [HttpPut("actualizarlibro/{id}")]
        public async Task<ActionResult> ActualizarLibro(int id, [FromBody] Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest("El ID del autor no coincide con el ID proporcionado.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(libro).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libro.Any(e => e.Id == id);
        }

        [HttpDelete("eliminarlibro/{id}")]
        public async Task<ActionResult> EliminarLibro(int id)
        {
            var libro = await _context.Libro.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            _context.Libro.Remove(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

