using System.ComponentModel.DataAnnotations;

namespace gestionSite.API.DTOs.FAQDtos
{
    public class AddFAQDto
    {
        [Required(ErrorMessage ="")]
        public string? Question { get; set; }
        [Required]
        public string? Response { get; set; }
        [Required]
        public int IdAdmin { get; set; }    
    }
}
