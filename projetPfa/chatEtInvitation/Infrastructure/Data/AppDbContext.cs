using chatEtInvitation.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace gestionEmployer.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AmisChat> AmisChats { get; set; }
        public DbSet<BloqueList> BloqueList { get; set; }
        public DbSet<ChatAmisMessage> ChatAmisMessages { get; set; }
        public DbSet<TeamChatMessage> TeamChatMessages { get; set; }
        public DbSet<MemberInvitation> MemberInvitations { get; set; }
        public DbSet<TeamInvitation> TeamInvitations { get; set; }
        public DbSet<TeamChat> TeamChats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table-Per-Concrete-Type (TPC) Inheritance
            modelBuilder.Entity<ChatAmisMessage>().UseTpcMappingStrategy();
            modelBuilder.Entity<TeamChatMessage>().UseTpcMappingStrategy();

            // Relationship: ChatAmisMessage -> AmisChat
            modelBuilder.Entity<ChatAmisMessage>()
                .HasOne(cam => cam._AmisChat)
                .WithMany()
                .HasForeignKey(cam => cam.ChatAmisId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: TeamChatMessage -> TeamChat
            modelBuilder.Entity<TeamChatMessage>()
                .HasOne(tcm => tcm._TeamChat)
                .WithMany()
                .HasForeignKey(tcm => tcm.TeamChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BloqueList>()
            .HasKey(bl => new { bl.Bloked, bl.BlokedBy }); // Composite PK

            base.OnModelCreating(modelBuilder);
        }
    }

}

