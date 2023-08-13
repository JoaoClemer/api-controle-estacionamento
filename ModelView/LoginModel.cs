using System.ComponentModel.DataAnnotations;

namespace ControleDeEstacionamento.ModelView
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
