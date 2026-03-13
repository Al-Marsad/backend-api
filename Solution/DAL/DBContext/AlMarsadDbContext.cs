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
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Incident> Incidents { get; set; }
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

            builder.Entity<Location>()
            .HasOne(l => l.City)
            .WithMany(c => c.Locations)
            .HasForeignKey(l => l.CityId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUser>()
            .HasIndex(U => U.NormalizedEmail).IsUnique();

            builder.Entity<AppUser>()
            .HasIndex(U => U.PhoneNumber).IsUnique();
            
            builder.Entity<AppUser>()
            .Property(U => U.PhoneNumber).IsRequired();

            builder.Entity<City>()
            .HasIndex(c => c.Name).IsUnique();

            builder.Entity<Incident>()
            .HasOne(i => i.InitialIncidentReport)
            .WithMany(i => i.Incidents)
            .HasForeignKey(i => i.InitialIncidentReportId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Incident>()
            .HasOne(i => i.FieldResearcher)
            .WithMany(f => f.Incidents)
            .HasForeignKey(i => i.FieldResearcherId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Incident>()
            .HasOne(i => i.Location)
            .WithMany(l => l.Incidents)
            .HasForeignKey(i => i.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FinalIncidentReport>()
            .HasOne(f => f.FieldResearcher)
            .WithMany(f => f.FinalIncidentReports)
            .HasForeignKey(f => f.FieldResearcherId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FinalIncidentReport>()
            .HasOne(f => f.Incident)
            .WithOne(i => i.FinalIncidentReport)
            .HasForeignKey<FinalIncidentReport>(f => f.IncidentId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
