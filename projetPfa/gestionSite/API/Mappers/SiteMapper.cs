using gestionSite.API.DTOs.FAQDtos;
using gestionSite.API.DTOs.SiteDtos;
using gestionSite.Core.Models;

namespace gestionSite.API.Mappers
{
    public class SiteMapper
    {

        public static Site UpdateSiteDtoToSite(UpdateSiteDto _updateSiteDto)
        {
            return new Site
            {
                Id = _updateSiteDto.Id,
                Name = _updateSiteDto.Name,
                Logo = _updateSiteDto.Logo,
                Description = _updateSiteDto.Description,
                Email = _updateSiteDto.Email,
                PhoneNumber = _updateSiteDto.PhoneNumber,
                AboutUs = _updateSiteDto.AboutUs,
                CouleurPrincipale = _updateSiteDto.CouleurPrincipale,
                CouleurSecondaire = _updateSiteDto.CouleurSecondaire,
                Background = _updateSiteDto.Background,
                Addresse = _updateSiteDto.Addresse,
                DomainName = _updateSiteDto.DomainName,
                IdAdmin = _updateSiteDto.IdAdmin,
            };
        }
    }
}
