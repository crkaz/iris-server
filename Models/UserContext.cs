using Microsoft.EntityFrameworkCore;


namespace iris_server.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base() { }

        // Database tables.
        public DbSet<User> Users { get; set; }
        public DbSet<CalendarEntry> Calendars { get; set; }
        public DbSet<PatientMessage> Messages { get; set; }
        public DbSet<PatientConfiguration> Configurations { get; set; }
        public DbSet<StickyNote> Stickies { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading.
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IrisDatabase;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}