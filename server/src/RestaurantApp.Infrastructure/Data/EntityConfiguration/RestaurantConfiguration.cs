using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.Id)
                   .IsUnique();
            
            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasOne(r => r.Account)
                   .WithOne(a => a.Restaurant)
                   .HasForeignKey<Restaurant>(r => new { r.AccountId, r.Type })
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
