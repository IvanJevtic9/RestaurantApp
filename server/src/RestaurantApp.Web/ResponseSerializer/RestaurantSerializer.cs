using System;
using System.Collections.Generic;

namespace RestaurantApp.Web.ResponseSerializer
{
    public class RestaurantGetSerializer
    {
        public int Id { get; }
        public List<MenuListSerializer> MenuCategories { get; }
        public List<GalleryImageSerializer> GalleryImages { get; }

        public RestaurantGetSerializer()
        {
            MenuCategories = new List<MenuListSerializer>();
            GalleryImages = new List<GalleryImageSerializer>();
        }
    }

    public class RestaurantListSerializer
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string ProfileUrl { get; }
        public string City { get; }
        public string Address { get; }
        public string PostalCode { get; }
        public string Phone { get; }
    }

    public class MenuListSerializer : MenuSerializer
    {
        public List<MenuItemSerializer> MenuItems { get; }
        public MenuListSerializer()
        {
            MenuItems = new List<MenuItemSerializer>();
        }
    }

    public class MenuSerializer
    {
        public int Id { get; }
        public string Name { get; }
        public string ImageUrl { get; }
    }

    public class MenuItemSerializer
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Attributes { get; }
        public double Price { get; }
        public string ImageUrl { get; }
    }

    public class GalleryImageSerializer
    {
        public int ImageId { get; }
        public string ImageUrl { get; }
    }

    public class PaymentOrderSerializer
    {
        public int Id { get; }
        public DateTime TimeCreated { get; }
        public DateTime? DeliveryTime { get; }
        public string PaymentItems { get; }
        public string State { get; }
        public double TotalPrice { get; }
        public RestaurantDet Restaurant { get; }
        public UserDet User { get; }
    }
    public class RestaurantDet
    {
        public int Id { get; }
        public string Name { get; }
        public string ProfileUrl { get; }
        public string City { get; }
        public string Address { get; }
        public string Phone { get; }
    }
    public class UserDet
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string ProfileUrl { get; }
        public string City { get; }
        public string Address { get; }
        public string Phone { get; }
    }
}
