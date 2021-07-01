using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Factory;
using RestaurantApp.Core.Lib;
using RestaurantApp.Core.Manager;
using RestaurantApp.Core.RepositoryInterface;
using RestaurantApp.Web.Authorization;
using RestaurantApp.Web.WebModel;
using RestaurantApp.Web.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.WebController
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthorizationService authorizationService;
        private readonly ImageManager imageManager;
        private readonly DynamicTypeFactory dynamicTypeFactory;
        public RestaurantController(DynamicTypeFactory dynamicTypeFactory,
                                    IUnitOfWork unitOfWork,
                                    IAuthorizationService authorizationService,
                                    ImageManager imageManager)
        {
            this.dynamicTypeFactory = dynamicTypeFactory;
            this.unitOfWork = unitOfWork;
            this.authorizationService = authorizationService;
            this.imageManager = imageManager;
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
            unitOfWork.SaveChanges();

            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;
            return Ok(response);
        }
    }
}
