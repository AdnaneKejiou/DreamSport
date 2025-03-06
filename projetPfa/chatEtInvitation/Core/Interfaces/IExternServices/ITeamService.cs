namespace chatEtInvitation.Core.Interfaces.IExternServices
{
    public interface ITeamService
    {
        Task<List<int>> FetchMembersAsync(int TeamId);
    }
}
