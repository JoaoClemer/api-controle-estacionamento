using ControleDeEstacionamento.Models;

namespace ControleDeEstacionamento.ModelView
{
    public class VehicleModel
    {
        public string LicensePlate { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime EntryTime { get; set; }
        public int ParkingId { get; set; }
    }
}
