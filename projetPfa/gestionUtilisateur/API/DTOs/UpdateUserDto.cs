using System.ComponentModel.DataAnnotations;

namespace gestionUtilisateur.API.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Le prenom est obligatoire")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "The username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "The birthday is required")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "The birthday is required")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "The Sexe is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirmed { get; set; }
        [Required(ErrorMessage = "required")]
        public int IdAdmin { get; set; }
    }
}
