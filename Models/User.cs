

namespace ControleDeEstacionamento.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public  Company Company { get; set; }
        public int CompanyId { get; set; }
    }
    public enum UserRole
    {
        Admin,
        VehicleRegister
    }
}
