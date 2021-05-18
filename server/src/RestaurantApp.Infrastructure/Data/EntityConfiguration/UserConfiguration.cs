using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(r => r.Id);

            /*Property configuration*/
            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.FirstName)
                   .IsRequired()
                   .HasMaxLength(60);

            builder.Property(r => r.LastName)
                   .IsRequired()
                   .HasMaxLength(60);

            /*Index configuration*/
            builder.HasIndex(r => r.Id)
                   .IsUnique();

            /*Foreign key configuration*/
            builder.HasOne(r => r.Account)
                   .WithOne(a => a.User)
                   .HasForeignKey<User>(r => new { r.AccountId, r.Type })
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
