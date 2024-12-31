using gestionSite.Core.Interfaces.SiteInterfaces;
using gestionSite.Core.Models;

namespace gestionSite.Infrastructure.Data.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly AppDbContext _context; // Replace with your DbContext name

        public SiteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Site>> GetAllComplexInfosAsync(int idAdmin)
        {
            return await _context.Sites
                                 .Where(f => f.IdAdmin == idAdmin)
                                 .ToListAsync();
        }
        public async Task<Site?> AddSiteAsync(Site _site)
        {
            try
            {
                var result = await _context.Sites.AddAsync(_site);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<Site?> UpdateSiteAsync(Site _site)
        {
            var existingSite = await _context.Sites.FindAsync(_site.Id);
            if (existingSite == null)
            {
                return null;
            }

            existingSite.Name = _site.Name;
            existingSite.Logo = _site.Logo;
            existingSite.Description = _site.Description;
            existingSite.Email = _site.Email;
            existingSite.PhoneNumber = _site.PhoneNumber;
            existingSite.AboutUs = _site.AboutUs;
            existingSite.CouleurPrincipale = _site.CouleurPrincipale;
            existingSite.CouleurSecondaire = _site.CouleurSecondaire;
            existingSite.Background = _site.Background;
            existingSite.Addresse = _site.Addresse;
            existingSite.DomainName = _site.DomainName;

            _context.Sites.Update(existingSite);
            await _context.SaveChangesAsync();
            return existingSite;
        }




    }
}
