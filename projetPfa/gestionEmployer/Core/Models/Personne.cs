using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;



namespace gestionEmployer.Core.Models
{
    public class Personne
    {
        [Required,MaxLength(50)]
        public string Nom { get; set; }
        [Required ,MaxLength(50)]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "le mdp est obligatoire")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(15)]
        
        public string PhoneNumber { get; set; }
    }
}
