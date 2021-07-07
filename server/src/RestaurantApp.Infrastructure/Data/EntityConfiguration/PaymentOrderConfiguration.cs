using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Core.Entity;
using System;

namespace RestaurantApp.Infrastructure.Data.EntityConfiguration
{
    public class PaymentOrderConfiguration : IEntityTypeConfiguration<PaymentOrder>
    {
        public void Configure(EntityTypeBuilder<PaymentOrder> builder)
        {
            builder.ToTable("PaymentOrder");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.PaymentItems)
                .IsRequired();

            builder.HasOne(r => r.Restaurant)
                   .WithMany()
                   .HasForeignKey(p => p.RestaurantId)
                   .IsRequired();

            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .IsRequired();

            builder.Property(r => r.TimeCreated)
                   .IsRequired();
        }
    }
}
