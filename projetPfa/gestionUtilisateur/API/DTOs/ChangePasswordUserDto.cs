using System.ComponentModel.DataAnnotations;

namespace gestionUtilisateur.API.DTOs
{
   
        public class ChangePasswordUserDto
    {
            [Required]
            public int AdminId { get; set; }

            [Required]
            public int UserId { get; set; }

            [Required]
            public string OldPassword { get; set; }

            [Required]
            [MinLength(6)]
            public string NewPassword { get; set; }
        }
    
}
