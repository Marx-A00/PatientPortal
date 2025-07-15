using Microsoft.EntityFrameworkCore;

namespace PatientPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add your DbSet properties here
        // Example: public DbSet<Patient> Patients { get; set; }
    }
}