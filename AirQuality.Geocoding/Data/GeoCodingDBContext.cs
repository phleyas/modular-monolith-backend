using AirQuality.Geocoding.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.Geocoding.Data
{
    internal class GeoCodingDBContext : DbContext
    {
        public DbSet<GeoCodingInfo> geoCodingInfos { get; set; }
        public DbSet<GeoLocation> geoLocations { get; set; }
        public GeoCodingDBContext(DbContextOptions<GeoCodingDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoLocation>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.HasMany(x => x.GeoCodingInfos)
                 .WithOne(x => x.GeoLocation)
                 .HasForeignKey(x => x.GeoLocationId)
                 .OnDelete(DeleteBehavior.Restrict);
                // Optional but recommended: unique index to avoid dupes
                e.HasIndex(x => new { x.Lat, x.Lon }).IsUnique();
            });
        }
    }
}
