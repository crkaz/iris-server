using iris_server.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iris_server.Models
{
    public class DbCtx : DbContext
    {
        public DbCtx() : base() { }

        // Database tables.
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Carer> Carers { get; set; }
        public DbSet<PatientNotes> PatientNotes { get; set; }
        public DbSet<PatientConfig> Configs { get; set; }
        public DbSet<CalendarEntry> Calendars { get; set; }
        public DbSet<PatientMessage> Messages { get; set; }
        public DbSet<StickyNote> Stickies { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<DbLog> DbLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading.
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TEST-iris-db;");
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=iris-db;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Converters
            // Cannot store complex types in database. These converters managage the conversion to and from.

            var stringArrayConverter = new ValueConverter<List<string>, string>(
                v => v.ToString(),
                v => v.Split(',', StringSplitOptions.None).ToList());

            ValueConverter featuresConverter = new ValueConverter<IList<IFeature>, string>(
                 v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                 v => JsonConvert.DeserializeObject<IList<IFeature>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            #endregion

            // Assigned patient ids in carer model.
            modelBuilder
                .Entity<Carer>()
                .Property(e => e.AssignedPatientIds)
                .HasConversion(stringArrayConverter);

            //  Reminders in calendar model.
            modelBuilder
                .Entity<CalendarEntry>()
                .Property(e => e.Reminders)
                .HasConversion(stringArrayConverter);

            // Features in patientconfig model.
            modelBuilder
                .Entity<PatientConfig>()
                .Property(e => e.EnabledFeatures)
                .HasConversion(featuresConverter);
        }
    }
}