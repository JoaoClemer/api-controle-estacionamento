using ControleDeEstacionamento.Models;

namespace ControleDeEstacionamento.ModelView
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int CompanyId { get; set; }

    }
}
