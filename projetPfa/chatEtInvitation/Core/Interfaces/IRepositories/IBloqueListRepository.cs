using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface IBloqueListRepository
    {
        Task<BloqueList> IsBlockedAsync(int Bloked, int BlokedBy);
    
    }
}
