using Database.Configuration;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TshirtConfig());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Tshirt> Tshirts { get; set; }

        public DbSet<PriceOffer> PriceOffers { get; set; }

        public DbSet<Customer> LoyalCustomers { get; set; }
    }
}
