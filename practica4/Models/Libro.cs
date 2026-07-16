using System.ComponentModel.DataAnnotations;
namespace practica4.Models
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres.")]
        public string Titulo { get; set; }

        [Range(1450, 2100, ErrorMessage = "El año de publicación debe estar entre 1450 y 2100.")]
        public int AnioPublicacion { get; set; } = 0;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El género debe tener entre 3 y 50 caracteres.")]
        public string Genero { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Los numeros de pagina deben ser mayor que 1.")]
        public int NumeroPaginas { get; set; } = 0;

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; } = 0.0m;

        public bool disponibilidad { get; set; } = true;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del autor debe ser mayor que 0.")]
        public int AutorId { get; set; } = 0;
    }
}
