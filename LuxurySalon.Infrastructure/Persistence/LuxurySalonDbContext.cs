using LuxurySalon.Application.Interfaces;
using LuxurySalon.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LuxurySalon.Infrastructure.Persistence
{
    public class LuxurySalonDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public LuxurySalonDbContext(DbContextOptions<LuxurySalonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services => Set<Service>();
        public DbSet<Stylist> Stylists => Set<Stylist>();
        public DbSet<StylistService> StylistServices => Set<StylistService>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ApplicationUser Table Name
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            // Service Configuration
            builder.Entity<Service>(entity =>
            {
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Price).HasPrecision(18, 2);
            });

            // Stylist Configuration
            builder.Entity<Stylist>(entity =>
            {
                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(50);
            });

            // StylistService (Many-to-Many)
            builder.Entity<StylistService>(entity =>
            {
                entity.HasKey(ss => new { ss.StylistId, ss.ServiceId });

                entity.HasOne(ss => ss.Stylist)
                    .WithMany(s => s.StylistServices)
                    .HasForeignKey(ss => ss.StylistId);

                entity.HasOne(ss => ss.Service)
                    .WithMany(s => s.StylistServices)
                    .HasForeignKey(ss => ss.ServiceId);
            });

            // Appointment Configuration
            builder.Entity<Appointment>(entity =>
            {
                entity.HasOne(a => a.Customer)
                    .WithMany()
                    .HasForeignKey(a => a.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(a => a.Stylist)
                    .WithMany(s => s.Appointments)
                    .HasForeignKey(a => a.StylistId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Service)
                    .WithMany(s => s.Appointments)
                    .HasForeignKey(a => a.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                // RowVersion for Concurrency
                entity.Property(a => a.RowVersion)
                    .IsRowVersion();

                // Composite Index: (StylistId, StartTime, EndTime)
                entity.HasIndex(a => new { a.StylistId, a.StartTime, a.EndTime })
                    .HasDatabaseName("IX_Appointment_Stylist_Schedule");
            });
        }
    }
}
