using AirQuality.OpenAQ.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AirQuality.OpenAQ.Data
{
    internal class OpenAQDbContext : DbContext
    {
        public DbSet<ParameterDTO> Parameters { get; set; }
        public DbSet<LocationDTO> Locations { get; set; }
        public DbSet<SensorDTO> Sensors { get; set; }

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

                e.OwnsMany(s => s.Licenses, b =>
                {
                    b.ToTable("Locations_Licenses");
                    b.WithOwner().HasForeignKey("LocationId");
                    b.HasKey("LocationId", "Id");
                    b.Property(p => p.Id).HasColumnName("LicenseId");
                    b.Property(p => p.Name).HasColumnName("LicenseName");

                    b.OwnsOne(p => p.Attribution, a =>
                    {
                        a.Property(x => x.Id).HasColumnName("AttributionId");
                        a.Property(x => x.Name).HasColumnName("AttributionName");
                        a.Property(x => x.Url).HasColumnName("AttributionUrl");
                    });
                });

                e.OwnsMany(s => s.Instruments, b =>
                {
                    b.ToTable("Locations_Instruments");
                    b.WithOwner().HasForeignKey("LocationId");
                    b.HasKey("LocationId", "Id");
                    b.Property(p => p.Id).HasColumnName("InstrumentId");
                    b.Property(p => p.Name).HasColumnName("InstrumentName");
                });
            });

            modelBuilder.Entity<SensorDTO>(s =>
            {
                s.ToTable("SensorDTO");
                s.HasKey(x => x.Id);

                s.Property(x => x.LocationId);

                s.HasOne(x => x.Parameter).WithMany();

                s.OwnsOne(x => x.DatetimeFirst);
                s.OwnsOne(x => x.DatetimeLast);
                s.OwnsOne(x => x.Coverage, cov =>
                {
                    cov.OwnsOne(c => c.DatetimeFrom);
                    cov.OwnsOne(c => c.DatetimeTo);
                });

                s.OwnsOne(x => x.Latest, l =>
                {
                    l.ToTable("SensorLatest");
                    l.WithOwner().HasForeignKey("SensorId");
                    l.Property(p => p.HasValueMarker).IsRequired().HasDefaultValue(true);
                    l.OwnsOne(p => p.Datetime);
                    l.OwnsOne(p => p.Coordinates);
                });

                s.OwnsOne(x => x.Summary);
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
