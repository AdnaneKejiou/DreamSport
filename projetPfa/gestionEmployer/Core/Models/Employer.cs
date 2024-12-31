using System.ComponentModel.DataAnnotations;


namespace gestionEmployer.Core.Models
{
    public class Employer : Personne
    {
        public int Id { get; set; }
        public string CIN { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Username { get; set; }
        public double Salaire { get; set; }
        public int IdAdmin { get; set; }
    }
}
