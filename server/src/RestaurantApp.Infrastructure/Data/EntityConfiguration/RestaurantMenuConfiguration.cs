using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class RestaurantMenuConfiguration : IEntityTypeConfiguration<RestaurantMenu>
    {
        public void Configure(EntityTypeBuilder<RestaurantMenu> builder)
        {
            builder.ToTable("Menus");

            builder.HasKey(r => r.Id);

            /*Property configuration*/
            builder.Property(r => r.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            /*Foreign key configuration*/
            builder.HasOne(r => r.Restaurant)
                   .WithMany(r => r.RestaurantMenus)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(r => r.RestaurantId)
                   .IsRequired();

            builder.HasOne(r => r.MenuBanner)
                   .WithMany()
                   .HasForeignKey(r => r.MenuBannerId);
        }
    }
}
