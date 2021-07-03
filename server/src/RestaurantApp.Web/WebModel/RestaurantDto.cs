using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.WebModel
{
    public class RestaurantMenuDto
    {
        public string Name { get; set; }
        public IFormFile ManuBanner { get; set; }
    }

    public class RestaurantMenuItemUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public double? Price { get; set; }
        public IFormFile ItemImage { get; set; }
    }

    public class RestaurantMenuItemDto : RestaurantMenuItemUpdateDto
    {
        public int MenuId { get; set; }
    }

    public class GalleryDto
    {
        public List<IFormFile> GalleryImages { get; set; }
    }

    public class GalleryDelDto
    {
        public List<int> GalleryIds { get; set; }
    }
}
