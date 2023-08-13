using ControleDeEstacionamento.Models;
using System.ComponentModel.DataAnnotations;

namespace ControleDeEstacionamento.ModelView
{
    public class ParkingModel
    {
        [Required]
        public int TotalParkingSpots { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}
