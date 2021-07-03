using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.ResponseSerializer
{
    public class RestaurantGetSerializer
    {
        public int Id { get; set; }
        public List<MenuListSerializer> MenuCategories { get; set; }
        public List<GalleryImageSerializer> GalleryImages {get;set;}

        public RestaurantGetSerializer()
        {
            MenuCategories = new List<MenuListSerializer>();
            GalleryImages = new List<GalleryImageSerializer>();
        }
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
}
