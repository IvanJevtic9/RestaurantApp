using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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

    public class PaymentOrderListDto
    {
        public List<PaymentOrderDto> PaymentOrders { get; set; }
    }

    public class PaymentOrderDto
    {
        public string PaymentItems { get; set; }
        public double TotalPrice { get; set; }
        public int RestaurantId { get; set; }
    }

    public class PaymentOrderTransitionDto
    {
        public string TransitionName { get; set; }
        public DateTime? DeliveryTime { get; set; }
    }
}
