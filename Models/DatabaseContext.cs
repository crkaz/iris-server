using Microsoft.EntityFrameworkCore;


namespace iris_server.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base() { }

        // Database tables.
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Carer> Carers { get; set; }
        public DbSet<CalendarEntry> Calendars { get; set; }
        public DbSet<PatientMessage> Messages { get; set; }
        public DbSet<StickyNote> Stickies { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<DbLog> DbLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading.
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=iris-db;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}