using Microsoft.EntityFrameworkCore;
using PartnerCommissionSystem.Models;

namespace PartnerCommissionSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<BusinessOwner> BusinessOwners { get; set; }
        public DbSet<ReferralCode> ReferralCodes { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Commission> Commissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Commission>()
                .Property(c => c.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
