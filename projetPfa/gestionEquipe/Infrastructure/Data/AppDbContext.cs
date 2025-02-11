using gestionEquipe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace gestionEquipe.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Equipe> Equipes { get; set; }
        
        

    }
}
