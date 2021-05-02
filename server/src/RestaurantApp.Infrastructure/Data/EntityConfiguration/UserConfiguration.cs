using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.Id)
                   .IsUnique();
            
            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.FirstName)
                   .IsRequired()
                   .HasMaxLength(60);

            builder.Property(r => r.LastName)
                   .IsRequired()
                   .HasMaxLength(60);

            builder.HasOne(r => r.Account)
                   .WithOne(a => a.User)
                   .HasForeignKey<User>(r => new { r.AccountId, r.Type })
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
