﻿using GasWeb.Domain.Comments.Entities;
using GasWeb.Domain.Franchises.Entities;
using GasWeb.Domain.GasStations.Entities;
using GasWeb.Domain.PriceSubmissions.Entities;
using GasWeb.Domain.Schedulers.Entities;
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
        internal DbSet<Franchise> Franchises { get; private set; }
        internal DbSet<Scheduler> Schedulers { get; private set; }
        internal DbSet<Comment> Comments { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(x => new { x.NameId, x.AuthenticationSchema }).IsUnique();
            });

            modelBuilder.AuditEntity<GasStation>(b =>
            {
                b.Property(x => x.Name).IsRequired();
                b.HasOne(x => x.Franchise).WithMany().HasForeignKey(x => x.FranchiseId);
                b.HasMany(x => x.SubmitedPrices).WithOne().HasForeignKey(x => x.GasStationId);
            });

            modelBuilder.AuditEntity<PriceSubmission>(b =>
            {
                b.HasIndex(x => new { x.GasStationId, x.CreatedByUserId, x.SubmissionDate, x.FuelType }).IsUnique();
                b.Property(x => x.SubmissionDate).HasColumnType("date");
                b.HasMany(x => x.Ratings).WithOne().HasForeignKey(x => x.PriceSubmissionId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PriceSubmissionRating>(b =>
            {
                b.HasKey(x => new { x.PriceSubmissionId, x.UserId });
                b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.AuditEntity<Franchise>(b =>
            {
                b.Property(x => x.Name).IsRequired();
                b.HasIndex(x => x.Name).IsUnique();
                b.HasMany(x => x.WholesalePrices).WithOne().HasForeignKey(x => x.FranchiseId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FranchiseWholesalePrice>(b =>
            {
                b.HasKey(x => new { x.FranchiseId, x.FuelType });
            });

            modelBuilder.AuditEntity<Scheduler>(b =>
            {
                b.Property(x => x.Id).ValueGeneratedNever();
                b.HasOne<Franchise>().WithMany().HasForeignKey(x => x.FranchiseId);
            });

            modelBuilder.AuditEntity<Comment>(b =>
            {
                b.Property(x => x.Content).IsRequired();
                b.Property(x => x.SubjectId).IsRequired();
            });
        }
    }
}
