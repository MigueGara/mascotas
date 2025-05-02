namespace SistemaAdopcionMascotas.Models
{
    public class ListarAdopcionesViewModel
    {
        public required string Mascota { get; set; }
        public required string Adoptante { get; set; }
        public required string EstadoAdopcion { get; set; }
    }
}