using GasWeb.Domain.GasStations.Entities;
using GasWeb.Domain.PriceSubmissions.Entities;
using GasWeb.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace GasWeb.Domain
{
    public class GasWebDbContext : DbContext
    {
        public GasWebDbContext(DbContextOptions<GasWebDbContext> options) : base(options)
        {
        }

        internal DbSet<User> Users { get; private set; }
        internal DbSet<GasStation> GasStations { get; private set; }
        internal DbSet<PriceSubmission> PriceSubmissions { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(x => x.Name).IsUnique();
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriceSubmission>(b =>
            {
                //b.HasIndex(x => new { x.GasStationId, x.CreatedByUserId, x.SubmissionDate, x.FuelType }).IsUnique();
                b.Property(x => x.SubmissionDate).HasColumnType("date");
            });
        }
    }
}
