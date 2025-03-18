namespace gestionEquipe.API.DTOs
{
    public class UserTeamMembershipResponseDto
    {
        public bool IsMember { get; set; } // True si l'utilisateur appartient à une équipe, sinon False
        public int? EquipeId { get; set; } // ID de l'équipe à laquelle l'utilisateur appartient (optionnel)
        public string EquipeNom { get; set; } // Nom de l'équipe (optionnel)
    }
}
