using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Factory;
using RestaurantApp.Core.Interface;
using RestaurantApp.Core.Lib;
using RestaurantApp.Core.Manager;
using RestaurantApp.Core.RepositoryInterface;
using RestaurantApp.Web.WebModel;
using RestaurantApp.Web.WebModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Web.WebController
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager<Account> accountManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ImageManager imageManager;
        private readonly DynamicTypeFactory dynamicTypeFactory;

        public AccountController(DynamicTypeFactory dynamicTypeFactory,
                                 IAccountManager<Account> accountManager,
                                 IUnitOfWork unitOfWork,
                                 ImageManager imageManager)
        {
            this.dynamicTypeFactory = dynamicTypeFactory;
            this.accountManager = accountManager;
            this.unitOfWork = unitOfWork;
            this.imageManager = imageManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromForm] AccountDto request)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var newAccount = new Account()
            {
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                Phone = request.Phone,
                AccountType = request.AccountType == AccountType.Restaurant.ToString() ? AccountType.Restaurant : AccountType.User
            };

            if (newAccount.AccountType == AccountType.Restaurant)
            {
                newAccount.Restaurant = new Restaurant()
                {
                    Name = request.Restaurant.Name,
                    Description = request.Restaurant.Description
                };
            }
            else
            {
                newAccount.User = new User()
                {
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    DateOfBirth = request.User.DateOfBirth
                };
            }

            var result = accountManager.CreateAccount(newAccount, request.Password);

            if (!result.Succeeded)
            {
                var dict = result.TransformToDict();
                response.Errors = dict.GetModelError(dynamicTypeFactory);
                return BadRequest(response);
            }

            if (request.ImageFile != null)
            {
                var image = new Image()
                {
                    ImangeName = request.ImageFile.FileName,
                    ImageLocation = "#PROFILE_PICTURE",
                    Role = ImageRole.Profile,
                    Title = request.ImageFile.FileName.Split('.')[0]
                };

                result = imageManager.UploadFile(image, request.ImageFile);

                if (!result.Succeeded)
                {
                    var dict = result.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);
                }

                newAccount.ImageId = image.Id;
                unitOfWork.SaveChanges();
            }

            response.Message = ResponseCodes.SUCCESSFUL_REGISTRATION;
            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var response = new ApiResponse();
            var dict = new Dictionary<string, List<string>>();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var account = accountManager.GetByEmail(request.Email);

            if (account == null)
            {
                dict.Add(nameof(request.Email), new List<string>() { ResponseCodes.INVALID_LOGIN });

                response.Errors = dict.GetModelError(dynamicTypeFactory);
                return BadRequest(response);
            }

            if (!accountManager.CheckPassword(account, request.Password))
            {
                dict.Add(nameof(request.Email), new List<string>() { ResponseCodes.INVALID_LOGIN });

                response.Errors = dict.GetModelError(dynamicTypeFactory);
                return BadRequest(response);
            }

            response.Data = accountManager.JwtProvider.GetJwtToken(account);
            return Ok(response);
        }
    }
}
