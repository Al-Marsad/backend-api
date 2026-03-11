using System.Reflection.Emit;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;


namespace DAL.DBContext
{
    public class AlMarsadDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<FieldResearcherInfo> FieldResearcherInfos { get; set; }
        public DbSet<InitialIncidentReport> InitialIncidentReports { get; set; }
        public DbSet<City> Cities { get; set; } 
        public AlMarsadDbContext(DbContextOptions<AlMarsadDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FieldResearcherInfo>()
            .HasOne(f => f.User)
            .WithOne(u => u.ResearcherInfo)
            .HasForeignKey<FieldResearcherInfo>(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InitialIncidentReport>()
            .HasOne(r => r.CitizenReporter)
            .WithMany(u => u.InitialIncidentReports)
            .HasForeignKey(r => r.CitizenReporterId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FieldResearcherInfo>()
            .HasOne(f => f.City)
            .WithMany(c => c.FieldResearchers)
            .HasForeignKey(f => f.CityId)
            .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
