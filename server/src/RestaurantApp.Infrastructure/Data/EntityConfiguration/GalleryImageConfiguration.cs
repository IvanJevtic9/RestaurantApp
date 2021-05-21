using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class GalleryImageConfiguration : IEntityTypeConfiguration<GalleryImage>
    {
        public void Configure(EntityTypeBuilder<GalleryImage> builder)
        {
            builder.ToTable("GalleryImages");

            builder.HasKey((g) => new { g.ImageId, g.RestaurantId });

            /*Index configuration*/
            builder.HasIndex(g => new { g.ImageId, g.RestaurantId })
                   .IsUnique();

            /*Foreign key configuration*/
            builder.HasOne(g => g.Image)
                   .WithMany()
                   .HasForeignKey(g => g.ImageId)
                   .IsRequired();

            builder.HasOne(g => g.Restaurant)
                   .WithMany(r => r.GalleryImages)
                   .HasForeignKey(g => g.RestaurantId)
                   .IsRequired();
        }
    }
}
