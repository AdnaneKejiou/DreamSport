using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IEquipeService
    {
        Task<AddedEquipeDTO> AddEquipeAsync(Equipe _equipe);

        // Méthode pour supprimer une équipe avec ses membres
        Task SupprimerEquipeAvecMembresAsync(int equipeId);

        // Méthode pour obtenir une équipe par son ID

    }
}
