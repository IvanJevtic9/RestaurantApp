using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(a => new { a.Id, a.AccountType });
            
            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Email)
                   .IsRequired()
                   .HasMaxLength(254);

            builder.Property(a => a.PasswordHash)
                   .IsRequired();

            builder.Property(a => a.AccountType)
                   .IsRequired();
            
            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(189);
            
            builder.Property(a => a.Address)
                   .IsRequired()
                   .HasMaxLength(254);
            
            builder.Property(a => a.PostalCode)
                   .IsRequired()
                   .HasMaxLength(10);
            
            builder.Property(a => a.Phone)
                   .HasMaxLength(15);

            builder.HasIndex(r => r.Id)
                   .IsUnique();

            builder.HasIndex(a => new { a.Id, a.AccountType })
                   .IsUnique();

            builder.HasIndex(a => a.Email)
                   .IsUnique();
        }
    }
}
