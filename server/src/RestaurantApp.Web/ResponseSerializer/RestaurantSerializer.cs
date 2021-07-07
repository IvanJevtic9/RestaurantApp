using System;
using System.Collections.Generic;

namespace RestaurantApp.Web.ResponseSerializer
{
    public class RestaurantGetSerializer
    {
        public int Id { get; set; }
        public List<MenuListSerializer> MenuCategories { get; set; }
        public List<GalleryImageSerializer> GalleryImages { get; set; }

        public RestaurantGetSerializer()
        {
            MenuCategories = new List<MenuListSerializer>();
            GalleryImages = new List<GalleryImageSerializer>();
        }
    }

    public class RestaurantListSerializer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfileUrl { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
    }

    public class MenuListSerializer : MenuSerializer
    {
        public List<MenuItemSerializer> MenuItems { get; set; }
        public MenuListSerializer()
        {
            MenuItems = new List<MenuItemSerializer>();
        }
    }

    public class MenuSerializer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class MenuItemSerializer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GalleryImageSerializer
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PaymentOrderSerializer
    {
        public int Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string PaymentItems { get; set; }
        public string State { get; set; }
        public double TotalPrice { get; set; }
        public List<string> AvailableTransitions { get; set; }
        public RestaurantDet Restaurant { get; set; }
        public UserDet User { get; set; }
    }
    public class RestaurantDet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class UserDet
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileUrl { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
