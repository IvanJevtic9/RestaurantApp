using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(i => i.Id);

            /*Property configuration*/
            builder.Property(i => i.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(i => i.ImageName)
                   .IsRequired()
                   .HasMaxLength(254);

            builder.Property(i => i.Url)
                   .IsRequired()
                   .HasMaxLength(254);

            builder.Property(i => i.Title)
                   .HasMaxLength(254);

            builder.Property(i => i.ImageLocation)
                  .HasMaxLength(254);

            builder.Property(i => i.Role)
                  .IsRequired();

            /*Index configuration*/
            builder.HasIndex(r => r.Id)
                   .IsUnique();
        }
    }
}
