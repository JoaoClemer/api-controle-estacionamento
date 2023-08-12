namespace ControleDeEstacionamento.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public int TotalParkingSpots { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }


    }
}
