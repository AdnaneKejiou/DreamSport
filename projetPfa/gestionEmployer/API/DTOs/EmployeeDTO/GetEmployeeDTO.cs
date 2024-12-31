using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class GetEmployeeDTO
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "L'ID doit être supérieur à 0.")]
        public int id {  get; set; }
    }
}
