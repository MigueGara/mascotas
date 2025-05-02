using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAdopcionMascotas.Models
{
    public class Mascota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Range(0, int.MaxValue)]
        public int Edad { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public string EstadoAdopcion { get; set; } // Disponible / Adoptada

        public Adopcion Adopcion { get; set; }
    }

    public class Adoptante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Contacto { get; set; }

        public ICollection<Adopcion> Adopciones { get; set; }
    }

    public class Adopcion
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Mascota")]
        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }

        [ForeignKey("Adoptante")]
        public int AdoptanteId { get; set; }
        public Adoptante Adoptante { get; set; }
    }
}