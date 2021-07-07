using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.WebModel
{
    public class RestaurantMenuDto
    {
        public string Name { get; }
        public IFormFile ManuBanner { get; }
    }

    public class RestaurantMenuItemUpdateDto
    {
        public string Name { get; }
        public string Description { get; }
        public string Attributes { get; }
        public double? Price { get; }
        public IFormFile ItemImage { get; }
    }

    public class RestaurantMenuItemDto : RestaurantMenuItemUpdateDto
    {
        public int MenuId { get; }
    }

    public class GalleryDto
    {
        public List<IFormFile> GalleryImages { get; }
    }

    public class GalleryDelDto
    {
        public List<int> GalleryIds { get; }
    }

    public class PaymentOrderListDto
    {
        public List<PaymentOrderDto> PaymentOrders { get; }
    }

    public class PaymentOrderDto
    {
        public string PaymentItems { get; }
        public double TotalPrice { get; }
        public int RestaurantId { get; }
    }

    public class PaymentOrderTransitionDto
    {
        public string TransitionName { get; }
        public DateTime? DeliveryTime { get; }
    }
}
