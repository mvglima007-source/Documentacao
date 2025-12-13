using System.ComponentModel.DataAnnotations;

namespace SistemaEspaco.Models.ViewModels
{
    public class DashboardReserva
    {
        [Key]
        public int TotalReservas { get; set; }
        public int ReservasAtivas { get; set; }
        public int ReservasFinalizadas { get; set; }
    }
}
