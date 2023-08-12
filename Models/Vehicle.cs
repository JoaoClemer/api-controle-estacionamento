namespace ControleDeEstacionamento.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public virtual Parking Parking { get; set; }
        public int ParkingId { get; set; }
    }
}
