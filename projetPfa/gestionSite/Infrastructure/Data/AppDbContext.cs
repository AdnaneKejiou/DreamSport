using gestionSite.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionSite.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }   
        
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Terrain> Terrains { get; set; }
        public DbSet<TerrainStatus> TerrainStatuses { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Annonces> Annonces { get; set; }

    }
}
