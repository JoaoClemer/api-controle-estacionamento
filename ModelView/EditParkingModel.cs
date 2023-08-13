using System.ComponentModel.DataAnnotations;

namespace ControleDeEstacionamento.ModelView
{
    public class EditParkingModel
    {
        [Required]
        public int TotalParkingSpots { get; set; }
    }
}
