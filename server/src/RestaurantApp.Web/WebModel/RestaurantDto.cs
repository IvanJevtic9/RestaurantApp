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
}
