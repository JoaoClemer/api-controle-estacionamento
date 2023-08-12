namespace ControleDeEstacionamento.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Parking> Parkings { get; set; }
    }

    public enum Country
    {
        Brazil,
        Argentina
    }
}
