using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Factory;
using RestaurantApp.Core.Lib;
using RestaurantApp.Core.Manager;
using RestaurantApp.Core.RepositoryInterface;
using RestaurantApp.Web.ResponseSerializer;
using RestaurantApp.Web.WebModel;
using RestaurantApp.Web.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.WebController
{
    [Route("api/restaurant")]
    [DisableRequestSizeLimit]
    public class RestaurantController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthorizationService authorizationService;
        private readonly IMapper mapper;
        private readonly ImageManager imageManager;
        private readonly DynamicTypeFactory dynamicTypeFactory;
        public RestaurantController(DynamicTypeFactory dynamicTypeFactory,
                                    IUnitOfWork unitOfWork,
                                    IAuthorizationService authorizationService,
                                    IMapper mapper,
                                    ImageManager imageManager)
        {
            this.dynamicTypeFactory = dynamicTypeFactory;
            this.unitOfWork = unitOfWork;
            this.authorizationService = authorizationService;
            this.mapper = mapper;
            this.imageManager = imageManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestourantDetail([FromRoute] int id)
        {
            var response = new ApiResponse();

            var restaurant = unitOfWork.Restaurant.GetById(id);

            if (restaurant == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound(response);
            }

            var data = mapper.Map<RestaurantGetSerializer>(restaurant);
            response.Data = data;

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurant([FromQuery] string query)
        {
            var response = new ApiResponse();

            var restaurants = unitOfWork.Restaurant.GetAll(r => r.Name.Contains(query) ||
                                                           r.Account.City.Contains(query) ||
                                                           r.Account.Address.Contains(query)).ToList();

            var data = mapper.Map<List<RestaurantListSerializer>>(restaurants);
            response.Data = data;

            return Ok(data);
        }

        [HttpPost("menu")]
        [Authorize]
        public async Task<IActionResult> CreateMenu([FromForm] RestaurantMenuDto request)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);

            var image = new Image()
            {
                ImageName = request.ManuBanner.FileName,
                ImageLocation = "#MENU_CATEGORY_BANNER",
                Role = ImageRole.Menu,
                Title = request.ManuBanner.FileName.Split('.')[0]
            };

            var res = imageManager.UploadFile(image, request.ManuBanner);
            if (!res.Succeeded)
            {
                var dict = res.TransformToDict();
                response.Errors = dict.GetModelError(dynamicTypeFactory);

                return BadRequest(response);
            }

            var menuCategory = new RestaurantMenu()
            {
                Name = request.Name,
                MenuBannerId = image.Id,
                RestaurantId = restaurant.Id
            };

            unitOfWork.Menu.Add(menuCategory);
            unitOfWork.SaveChanges();

            var data = mapper.Map<MenuSerializer>(menuCategory);
            response.Data = data;
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;

            return Ok(response);
        }

        [HttpPut("menu/{menuId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenu([FromForm] RestaurantMenuDto request, [FromRoute] int menuId)
        {
            var response = new ApiResponse();

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);
            var menuCategory = unitOfWork.Menu.GetById(menuId);

            if (menuCategory == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound(response);
            }

            if (restaurant.Id != menuCategory.RestaurantId)
            {
                return Forbid();
            }

            if (request.ManuBanner != null)
            {
                var image = new Image()
                {
                    ImageName = request.ManuBanner.FileName,
                    ImageLocation = "#MENU_CATEGORY_BANNER",
                    Role = ImageRole.Menu,
                    Title = request.ManuBanner.FileName.Split('.')[0]
                };

                var res = imageManager.UploadFile(image, request.ManuBanner);
                if (!res.Succeeded)
                {
                    var dict = res.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);

                    return BadRequest(response);
                }
                var tmpId = menuCategory.MenuBannerId;
                menuCategory.MenuBannerId = image.Id;
                imageManager.DeleteFile((int)tmpId);
            }
            if (request.Name != null)
            {
                menuCategory.Name = request.Name;
            }

            unitOfWork.SaveChanges();

            var data = mapper.Map<MenuSerializer>(menuCategory);
            response.Data = data;
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;

            return Ok(response);
        }

        [HttpDelete("menu/{menuId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenu([FromRoute] int menuId)
        {
            var response = new ApiResponse();

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);
            var menuCategory = unitOfWork.Menu.GetById(menuId);

            if (menuCategory == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound(response);
            }

            if (restaurant.Id != menuCategory.RestaurantId)
            {
                return Forbid();
            }

            imageManager.DeleteFile((int)menuCategory.MenuBannerId);
            unitOfWork.Menu.Remove(menuCategory);

            /*TODO remove all images from Menu items from wwwroot*/

            unitOfWork.SaveChanges();

            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;
            return Ok(response);
        }

        [HttpPost("menu-item")]
        [Authorize]
        public async Task<IActionResult> CreateMenuItem([FromForm] RestaurantMenuItemDto request)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);
            var menu = unitOfWork.Menu.GetFirstOrDefault(m => m.Id == request.MenuId);

            if (menu == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound(response);
            }

            if (restaurant.Id != menu.RestaurantId)
            {
                return Forbid();
            }

            var item = new MenuItem()
            {
                Name = request.Name,
                ItemDescription = request.Description,
                Attributes = request.Attributes,
                Price = (int)request.Price,
                MenuId = request.MenuId
            };

            if (request.ItemImage != null)
            {
                var image = new Image()
                {
                    ImageName = request.ItemImage.FileName,
                    ImageLocation = "#MENU_ITEM_PICTURE",
                    Role = ImageRole.MenuItem,
                    Title = request.ItemImage.FileName.Split('.')[0]
                };

                var res = imageManager.UploadFile(image, request.ItemImage);
                if (!res.Succeeded)
                {
                    var dict = res.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);

                    return BadRequest(response);
                }

                item.ItemImageId = image.Id;
            }

            unitOfWork.MenuItem.Add(item);
            unitOfWork.SaveChanges();

            var data = mapper.Map<MenuItemSerializer>(item);
            response.Data = data;
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;

            return Ok(response);
        }

        [HttpPut("menu-item/{itemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenuItem([FromForm] RestaurantMenuItemUpdateDto request, [FromRoute] int itemId)
        {
            var response = new ApiResponse();

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);
            var menuItem = unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == itemId);

            if (menuItem == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound(response);
            }

            if (restaurant.Id != menuItem.Menu.RestaurantId)
            {
                return Forbid();
            }

            if (request.Name != null)
            {
                menuItem.Name = request.Name;
            }
            if (request.Description != null)
            {
                menuItem.ItemDescription = request.Description;
            }
            if (request.Attributes != null)
            {
                menuItem.Attributes = request.Attributes;
            }
            if (request.Price != null)
            {
                menuItem.Price = (int)request.Price;
            }
            if (request.ItemImage != null)
            {
                var image = new Image()
                {
                    ImageName = request.ItemImage.FileName,
                    ImageLocation = "#MENU_ITEM_PICTURE",
                    Role = ImageRole.MenuItem,
                    Title = request.ItemImage.FileName.Split('.')[0]
                };

                var res = imageManager.UploadFile(image, request.ItemImage);
                if (!res.Succeeded)
                {
                    var dict = res.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);

                    return BadRequest(response);
                }
                var tmpId = menuItem.ItemImageId;
                menuItem.ItemImageId = image.Id;
                if (tmpId != null) imageManager.DeleteFile((int)tmpId);
            }

            unitOfWork.SaveChanges();

            var data = mapper.Map<MenuItemSerializer>(menuItem);
            response.Data = data;
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;

            return Ok(response);
        }

        [HttpDelete("menu-item/{itemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenuItem([FromRoute] int itemId)
        {
            var response = new ApiResponse();

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);
            var menuItem = unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == itemId);

            if (menuItem == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound(response);
            }

            if (restaurant.Id != menuItem.Menu.RestaurantId)
            {
                return Forbid();
            }

            imageManager.DeleteFile((int)menuItem.ItemImageId);
            unitOfWork.MenuItem.Remove(menuItem);
            unitOfWork.SaveChanges();

            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;
            return Ok(response);
        }

        [HttpPost("gallery")]
        [Authorize]
        public async Task<IActionResult> UploadGalleryImage([FromForm] GalleryDto request)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (accountType == AccountType.User.ToString())
            {
                return Forbid();
            }

            var restaurant = unitOfWork.Restaurant.GetFirstOrDefault(r => r.AccountId == userId);

            var galleryImages = new List<GalleryImage>();
            foreach (var item in request.GalleryImages)
            {
                var image = new Image()
                {
                    ImageName = item.FileName,
                    ImageLocation = "#GALLERY_PICTURE",
                    Role = ImageRole.Gallery,
                    Title = item.FileName.Split('.')[0]
                };

                var res = imageManager.UploadFile(image, item);
                if (!res.Succeeded)
                {
                    var dict = res.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);

                    return BadRequest(response);
                }

                galleryImages.Add(new GalleryImage()
                {
                    ImageId = image.Id,
                    RestaurantId = restaurant.Id
                });
            }

            unitOfWork.GalleryImage.AddRange(galleryImages);
            unitOfWork.SaveChanges();

            var data = mapper.Map<List<GalleryImageSerializer>>(galleryImages);
            response.Data = data;
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;

            return Ok(response);
        }

        [HttpDelete("gallery")]
        [Authorize]
        public async Task<IActionResult> UploadGalleryImage([FromBody] GalleryDelDto request)
        {
            var response = new ApiResponse();

            foreach (var id in request.GalleryIds)
            {
                var image = unitOfWork.Image.GetById(id);
                imageManager.DeleteFile(image);
                unitOfWork.GalleryImage.Remove(id);
            }

            unitOfWork.SaveChanges();
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;

            return Ok(response);
        }

        [HttpPost("payment-order")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentOrder([FromBody] PaymentOrderListDto request)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("userId")).Value);

            if (userId == null)
            {
                return Forbid();
            }

            foreach (var order in request.PaymentOrders)
            {
                var restaurant = unitOfWork.Restaurant.GetById(order.RestaurantId);
                if (restaurant == null)
                {
                    var dict = new Dictionary<string, List<string>>();
                    dict.Add("restaurantId", new List<string>() { ResponseCodes.DOES_NOT_EXIST });

                    response.Errors = dict.GetModelError(dynamicTypeFactory);
                    return BadRequest(response);
                }

                var paymentOrder = new PaymentOrder
                {
                    PaymentItems = order.PaymentItems,
                    RestaurantId = order.RestaurantId,
                    TotalPrice = order.TotalPrice,
                    TimeCreated = DateTime.Now,
                    UserId = userId
                };

                unitOfWork.PaymentOrder.Add(paymentOrder);
            }

            unitOfWork.SaveChanges();

            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;
            return Ok(response);
        }

        [HttpPut("payment-order/{id}")]
        [Authorize]
        public async Task<IActionResult> PaymentOrderTransition([FromBody] PaymentOrderTransitionDto request, [FromRoute] int id)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var paymentOrder = unitOfWork.PaymentOrder.GetById(id);
            if (paymentOrder == null)
            {
                response.Message = ResponseCodes.DOES_NOT_EXIST;
                return NotFound();
            }

            try
            {
                var accountType = (AccountType)(object)this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;
                paymentOrder.MakeTransition(accountType, request.TransitionName);
                if (accountType == AccountType.Restaurant) paymentOrder.DeliveryTime = request.DeliveryTime;
            }
            catch (InvalidOperationException ex)
            {
                var dict = new Dictionary<string, List<string>>();
                dict.Add("restaurantId", new List<string>() { ex.Message });

                return BadRequest(dict.GetModelError(dynamicTypeFactory));
            }

            unitOfWork.SaveChanges();

            var data = mapper.Map<PaymentOrderSerializer>(paymentOrder);
            response.Data = data;

            return Ok(response);
        }

        [HttpGet("payment-order")]
        [Authorize]
        public async Task<IActionResult> GetPaymentOrder([FromQuery] bool active, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            var response = new ApiResponse();

            var userId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("userId")).Value);
            var restaurantId = Convert.ToInt32(this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("restaurantId")).Value);
            var accountType = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            List<PaymentOrder> po = new List<PaymentOrder>();

            if (accountType == AccountType.User.ToString())
            {
                po = unitOfWork.PaymentOrder.GetAll(x => x.UserId == userId).ToList();
            }
            else
            {
                po = unitOfWork.PaymentOrder.GetAll(x => x.RestaurantId == restaurantId).ToList();
            }

            if (active)
            {
                po.Where(x => active ? x.State == PaymentOrderState.Draft : x.State != PaymentOrderState.Draft).ToList();
            }
            if (dateFrom != null)
            {
                po.Where(x => x.TimeCreated >= dateFrom).ToList();
            }
            if (dateTo != null)
            {
                po.Where(x => x.TimeCreated <= dateTo).ToList();
            }

            var data = mapper.Map<List<PaymentOrderSerializer>>(po);
            response.Data = data;

            return Ok(response);
        }
    }
}
