using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");

            builder.HasKey(d => d.Id);

            /*Property configuration*/
            builder.Property(d => d.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(d => d.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(d => d.DishDescription)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(d => d.TagPrice)
                   .HasMaxLength(1000)
                   .IsRequired();

            /*Foreign key configuration*/
            builder.HasOne(d => d.ItemImage)
                   .WithMany()
                   .HasForeignKey(d => d.ItemImageId);

            builder.HasOne(d => d.Menu)
                   .WithMany(m => m.Items)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(d => d.MenuId)
                   .IsRequired();
        }
    }
}
