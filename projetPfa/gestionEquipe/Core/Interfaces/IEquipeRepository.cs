using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IEquipeRepository
    {
        Task<Equipe> AddEquipeAsync(Equipe _equipe);
        Task<int> CountEquipesBySportAndUser(int userId,int SportId);
        Task<bool> ExistWithName(string name, int AdminID);
        Task<bool> ExistWithIdAsync(int  id);
        Task<bool> IsCaptainAsync(int CaptainID, int EquipeId);
    }
}
