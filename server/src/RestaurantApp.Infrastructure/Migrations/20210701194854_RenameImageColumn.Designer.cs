// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestaurantApp.Infrastructure.Data;

namespace RestaurantApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210701194854_RenameImageColumn")]
    partial class RenameImageColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RestaurantApp.Core.Entity.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(189)
                        .HasColumnType("nvarchar(189)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id", "AccountType");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ImageId");

                    b.HasIndex("Id", "AccountType")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.GalleryImage", b =>
                {
                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.HasKey("ImageId", "RestaurantId");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("ImageId", "RestaurantId")
                        .IsUnique();

                    b.ToTable("GalleryImages");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImageLocation")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Images");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DishDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("ItemImageId")
                        .HasColumnType("int");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TagPrice")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("ItemImageId");

                    b.HasIndex("MenuId");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("AccountId", "Type")
                        .IsUnique();

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.RestaurantMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MenuBannerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MenuBannerId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("AccountId", "Type")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.Account", b =>
                {
                    b.HasOne("RestaurantApp.Core.Entity.Image", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.GalleryImage", b =>
                {
                    b.HasOne("RestaurantApp.Core.Entity.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantApp.Core.Entity.Restaurant", "Restaurant")
                        .WithMany("GalleryImages")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.MenuItem", b =>
                {
                    b.HasOne("RestaurantApp.Core.Entity.Image", "ItemImage")
                        .WithMany()
                        .HasForeignKey("ItemImageId");

                    b.HasOne("RestaurantApp.Core.Entity.RestaurantMenu", "Menu")
                        .WithMany("Items")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemImage");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.Restaurant", b =>
                {
                    b.HasOne("RestaurantApp.Core.Entity.Account", "Account")
                        .WithOne("Restaurant")
                        .HasForeignKey("RestaurantApp.Core.Entity.Restaurant", "AccountId", "Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.RestaurantMenu", b =>
                {
                    b.HasOne("RestaurantApp.Core.Entity.Image", "MenuBanner")
                        .WithMany()
                        .HasForeignKey("MenuBannerId");

                    b.HasOne("RestaurantApp.Core.Entity.Restaurant", "Restaurant")
                        .WithMany("RestaurantMenus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuBanner");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.User", b =>
                {
                    b.HasOne("RestaurantApp.Core.Entity.Account", "Account")
                        .WithOne("User")
                        .HasForeignKey("RestaurantApp.Core.Entity.User", "AccountId", "Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.Account", b =>
                {
                    b.Navigation("Restaurant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.Restaurant", b =>
                {
                    b.Navigation("GalleryImages");

                    b.Navigation("RestaurantMenus");
                });

            modelBuilder.Entity("RestaurantApp.Core.Entity.RestaurantMenu", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
