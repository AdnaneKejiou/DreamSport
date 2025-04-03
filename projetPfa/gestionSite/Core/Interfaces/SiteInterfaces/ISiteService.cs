using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.Core.Models;

namespace gestionSite.Core.Interfaces.SiteInterfaces
{
    public interface ISiteService
    {
        Task<IEnumerable<gestionSite.Core.Models.Site>> GetSiteByAdminAsync(int adminId);
        Task<gestionSite.Core.Models.Site?> AddSiteAsync(int adminId);
        Task<gestionSite.Core.Models.Site?> UpdateSiteAsync(gestionSite.Core.Models.Site site);
        Task<Site> GetSiteASync(int adminId);
    }
}
