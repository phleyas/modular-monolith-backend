using AirQuality.OpenAQ.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AirQuality.OpenAQ.Data
{
    internal class OpenAQDbContext : DbContext
    {
        public DbSet<ParameterDTO> Parameters { get; set; }
        public DbSet<LocationDTO> Locations { get; set; }

        public OpenAQDbContext(DbContextOptions<OpenAQDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParameterDTO>().HasKey(g => g.Id);

            modelBuilder.Entity<LocationDTO>(e =>
            {
                e.ToTable("Locations");
                e.HasKey(s => s.Id);

                e.OwnsOne(s => s.Coordinates);
                e.OwnsOne(s => s.Country);
                e.OwnsOne(s => s.Owner);
                e.OwnsOne(s => s.Provider);
                e.OwnsOne(s => s.DatetimeFirst);
                e.OwnsOne(s => s.DatetimeLast);

                // Sensors are regular child entities (keep if IDs are unique per DB)
                e.HasMany(s => s.Sensors)
                 .WithOne()
                 .HasForeignKey("LocationId")
                 .OnDelete(DeleteBehavior.Cascade); // ensure DB cascade

                // Licenses as owned collection to avoid global PK clashes on Id
                e.OwnsMany(s => s.Licenses, b =>
                {
                    b.ToTable("Locations_Licenses");
                    b.WithOwner().HasForeignKey("LocationId");
                    b.HasKey("LocationId", "Id"); // composite key scoped to owner
                    b.Property(p => p.Id).HasColumnName("LicenseId");
                    b.Property(p => p.Name).HasColumnName("LicenseName");
                    b.Property(p => p.DateFrom);
                    b.Property(p => p.DateTo);

                    // Nested owned object for attribution
                    b.OwnsOne(p => p.Attribution, a =>
                    {
                        a.Property(x => x.Id).HasColumnName("AttributionId");
                        a.Property(x => x.Name).HasColumnName("AttributionName");
                        a.Property(x => x.Url).HasColumnName("AttributionUrl");
                    });
                });

                // Owned collection with explicit table/FK/PK for instruments
                e.OwnsMany(s => s.Instruments, b =>
                {
                    b.ToTable("Locations_Instruments");
                    b.WithOwner().HasForeignKey("LocationId");
                    b.HasKey("LocationId", "Id");
                    b.Property(p => p.Id).HasColumnName("InstrumentId");
                    b.Property(p => p.Name).HasColumnName("InstrumentName");
                });
            });

            var utcConverter = new ValueConverter<DateTime, DateTime>(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var utcNullableConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    if (prop.ClrType == typeof(DateTime))
                        prop.SetValueConverter(utcConverter);
                    else if (prop.ClrType == typeof(DateTime?))
                        prop.SetValueConverter(utcNullableConverter);
                }
            }
        }
    }
}
