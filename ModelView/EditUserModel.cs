using System.ComponentModel.DataAnnotations;

namespace ControleDeEstacionamento.ModelView
{
    public class EditUserModel
    {
        
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
