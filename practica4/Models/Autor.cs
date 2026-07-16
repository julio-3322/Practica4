using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practica4.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Debe tener un máximo de 50 caracteres.")]
        public string Nacionalidad { get; set; }

        [Required]
        [Range(1500, 2100, ErrorMessage = "El año de nacimiento debe estar entre 1500 y 2100.")]
        public int anioNacimiento { get; set; }

    }
}
