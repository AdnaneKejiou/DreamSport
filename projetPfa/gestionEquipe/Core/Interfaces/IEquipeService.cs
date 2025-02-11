using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IEquipeService
    {
        Task<AddedEquipeDTO> AddEquipeAsync(Equipe _equipe);

    }
}
