namespace gestionUtilisateur.API.DTOs
{
    public class ReturnForgotPasswordDTO
    {
        public string Email { get; set; }
        public int idAdmin { get; set; }
        public string? error { get; set; }
    }
}
