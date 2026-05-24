using System.Reflection.Emit;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DAL.DBContext
{
    public class AlMarsadDbContext : 
        IdentityDbContext<
            AppUser,
            AppRole,
            string,
            IdentityUserClaim<string>,
            AppUserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>
        >
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }
        public DbSet<InitialIncidentReport> InitialIncidentReports { get; set; }
        public DbSet<City> Cities { get; set; } 
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<LegalTeamMemberNote> LegalTeamMemberNotes { get; set; }
        public DbSet<NewsItem> News { get; set; }
        public DbSet<Victim> Victims { get; set; }
        public DbSet<PersonalVictimTestimonie> PersonalVictimTestimonies { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<IncidentClass> IncidentClasses { get; set; }
        public DbSet<IncidentClassType> IncidentClassTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }


        public AlMarsadDbContext(DbContextOptions<AlMarsadDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });

            builder.Entity<InitialIncidentReport>()
            .HasOne(r => r.CitizenReporter)
            .WithMany(u => u.InitialIncidentReportsForCitizen)
            .HasForeignKey(r => r.CitizenReporterId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InitialIncidentReport>()
           .HasOne(r => r.FieldResearcher)
           .WithMany(u => u.AssignedInitialReportsForFieldResearcher)
           .HasForeignKey(r => r.FieldResearcherId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InitialIncidentReport>()
            .HasOne(r => r.City)
            .WithMany(u => u.InitialIncidentReports)
            .HasForeignKey(r => r.CityId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InitialIncidentReport>()
            .Property(i => i.CityId).HasDefaultValue(1);


            builder.Entity<AppUser>()
            .HasOne(f => f.City)
            .WithMany(c => c.Users)
            .HasForeignKey(f => f.CityId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<AppUser>()
            .HasIndex(U => U.NormalizedEmail).IsUnique();

            builder.Entity<AppUser>()
            .HasIndex(U => U.PhoneNumber).IsUnique();
            
            builder.Entity<AppUser>()
            .Property(U => U.PhoneNumber).IsRequired();

            builder.Entity<City>()
            .HasIndex(c => c.ArabicName).IsUnique();

            builder.Entity<City>()
            .HasIndex(c => c.EnglishName).IsUnique();

            builder.Entity<Incident>()
            .HasOne(i => i.InitialIncidentReport)
            .WithOne(i => i.Incident)
            .HasForeignKey<Incident>(i => i.InitialIncidentReportId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Incident>()
            .HasOne(i => i.FieldResearcher)
            .WithMany(f => f.IncidentsForFieldResearcher)
            .HasForeignKey(i => i.FieldResearcherId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Incident>()
            .HasOne(i => i.LegalTeamMember)
            .WithMany(f => f.AssignedIncidentsForLegalTeamMember)
            .HasForeignKey(i => i.LegalTeamMemberId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Incident>()
            .HasOne(i => i.City)
            .WithMany(f => f.Incidents)
            .HasForeignKey(i => i.CityId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Incident>()
                .Property(i => i.QuestionnaireJSON)
                .HasColumnType("jsonb");    


            builder.Entity<LegalTeamMemberNote>()
            .HasOne(n => n.Incident)
            .WithMany(i => i.LegalTeamMemberNotes)
            .HasForeignKey(n => n.IncidentId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<LegalTeamMemberNote>()
            .HasOne(l => l.LegalTeamMember)
            .WithMany(u => u.LegalTeamMemberNotes)
            .HasForeignKey(l => l.LegalTeamMemberId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NewsItem>()
            .HasOne(n => n.WrittenBy)
            .WithMany(u => u.News)
            .HasForeignKey(n => n.WrittenById)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NewsItem>()
            .HasOne(n => n.Incident)
            .WithOne(i => i.NewsItem)
            .HasForeignKey<NewsItem>(n => n.IncidentId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Victim>()
            .HasIndex(v => v.NationalId).IsUnique();

            builder.Entity<Victim>()
            .HasIndex(v => v.PhoneNumber).IsUnique();

            builder.Entity<PersonalVictimTestimonie>()
            .HasOne(t => t.Incident)
            .WithMany(i => i.PersonalVictimTestimonies)
            .HasForeignKey(t => t.IncidentId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<PersonalVictimTestimonie>()
            .HasOne(t => t.Victim)
            .WithMany(i => i.PersonalVictimTestimonies)
            .HasForeignKey(t => t.VictimId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Evidence>()
            .HasOne(e => e.Incident)
            .WithMany(i => i.Evidences)
            .HasForeignKey(e => e.IncidentId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Activity>()
            .HasOne(a => a.MadeBy)
            .WithMany(i => i.Activities)
            .HasForeignKey(a => a.MadeById)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
