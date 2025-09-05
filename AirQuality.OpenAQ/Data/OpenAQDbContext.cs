using AirQuality.OpenAQ.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.OpenAQ.Data
{
    internal class OpenAQDbContext : DbContext
    {
        public DbSet<ParameterDTO> Parameters { get; set; }
        public OpenAQDbContext(DbContextOptions<OpenAQDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParameterDTO>()
                .HasKey(g => g.Id);
        }
    }
}
