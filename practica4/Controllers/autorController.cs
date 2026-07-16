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
    public class autorController : Controller
    {
        private readonly AppDbContext _context;
        public autorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getautor")]
        public async Task<ActionResult> GetAutor()
        {
            var autores = await _context.Autor.ToListAsync();
            return Ok(autores);
        }

        [HttpGet("getautor/{id}")]
        public async Task<ActionResult> GetAutorporId(int id)
        {
            var autor = await _context.Autor.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpGet("{id:int}/getlibrosporautor")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosporAutor (int id)
        {
            var autorexiste = await _context.Autor.AnyAsync(a => a.Id == id);
            if (!autorexiste) return NotFound(new { mensaje = "El autor no existe" });

            var libros = await _context.Libro.Where(l => l.AutorId == id).ToListAsync();
            return Ok(libros);

        }

        [HttpGet("autorespaginado")]
        public async Task<ActionResult> GetAutorPaginado (int pag = 1, int sizepag = 5, string? busqueda = null
            ,string? ordenarpor = "anioNacimiento", string direccion = "ascender")
        {
            if (pag <= 0 || sizepag <= 0)
            {
                return BadRequest(new {mensaje = "Debes introducir un numero mayor o igual a 1"});
            }

            var consulta = _context.Autor.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(a => a.Nombre.Contains(busqueda));
            }

            consulta = direccion.ToLower() == "descender"
                ? consulta.OrderByDescending(a => EF.Property<Object>(a, ordenarpor))
                : consulta.OrderBy(a =>  EF.Property<Object>(a, ordenarpor));

            var autores = await consulta
                .Skip((pag - 1) * sizepag)
                .Take(sizepag)
                .ToListAsync();

            var resultado = new
            {
                pag,
                sizepag,
                datos = autores
            };

            return Ok(resultado);
        }

        [HttpPost("crearautor")]
        public async Task<ActionResult> CrearAutor([FromBody] Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Autor.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAutorporId), new { id = autor.Id }, autor);
        }

        [HttpPut("actualizarautor/{id}")]
        public async Task<ActionResult> ActualizarAutor(int id, [FromBody] Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("El ID del autor no coincide con el ID proporcionado.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(autor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
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

        private bool AutorExists(int id)
        {
            return _context.Autor.Any(e => e.Id == id);
        }

        [HttpDelete("eliminarautor/{id}")]
        public async Task<ActionResult> EliminarAutor(int id)
        {
            var autor = await _context.Autor.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            _context.Autor.Remove(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}