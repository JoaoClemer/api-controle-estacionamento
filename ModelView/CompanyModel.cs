using ControleDeEstacionamento.Models;
using System.ComponentModel.DataAnnotations;

namespace ControleDeEstacionamento.ModelView
{
    public class CompanyModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Country Country { get; set; }
    }
}
