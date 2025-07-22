using Microsoft.EntityFrameworkCore;
using PatientPortal.Models;

namespace PatientPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
            .HasOne(p => p.Patient)
            .WithMany(patient => patient.Payments)
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);
        }

        // Add your DbSet properties here
        // Example: public DbSet<Patient> Patients { get; set; }
    }
}