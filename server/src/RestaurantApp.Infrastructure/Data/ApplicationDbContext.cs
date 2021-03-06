using Microsoft.EntityFrameworkCore;
using RestaurantApp.Infrastructure.Data.EntityConfiguration;

namespace RestaurantApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new RestaurantConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new RestaurantMenuConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemConfiguration());
            modelBuilder.ApplyConfiguration(new GalleryImageConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentOrderConfiguration());
        }
    }
}
