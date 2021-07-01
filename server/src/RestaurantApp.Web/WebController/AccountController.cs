using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Factory;
using RestaurantApp.Core.Interface;
using RestaurantApp.Core.Lib;
using RestaurantApp.Core.Manager;
using RestaurantApp.Core.Model;
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
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager<Account> accountManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthorizationService authorizationService;
        private readonly ImageManager imageManager;
        private readonly DynamicTypeFactory dynamicTypeFactory;

        public AccountController(DynamicTypeFactory dynamicTypeFactory,
                                 IAccountManager<Account> accountManager,
                                 IUnitOfWork unitOfWork,
                                 IAuthorizationService authorizationService,
                                 ImageManager imageManager)
        {
            this.dynamicTypeFactory = dynamicTypeFactory;
            this.accountManager = accountManager;
            this.unitOfWork = unitOfWork;
            this.imageManager = imageManager;
            this.authorizationService = authorizationService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromForm] AccountDto request)
        {
            var response = new ApiResponse();
            var result = new OperationResult();

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

            if (request.ImageFile != null)
            {
                var image = new Image()
                {
                    ImageName = request.ImageFile.FileName,
                    ImageLocation = "#PROFILE_PICTURE",
                    Role = ImageRole.Profile,
                    Title = request.ImageFile.FileName.Split('.')[0]
                };

                result = imageManager.UploadFile(image, request.ImageFile);

                if (!result.Succeeded)
                {
                    var dict = result.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);
                    return BadRequest(response);
                }

                newAccount.ImageId = image.Id;
            }

            result = accountManager.CreateAccount(newAccount, request.Password);

            if (!result.Succeeded)
            {
                var dict = result.TransformToDict();
                response.Errors = dict.GetModelError(dynamicTypeFactory);
                return BadRequest(response);
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

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] AccountUpdateDto request, [FromRoute] int id)
        {
            var response = new ApiResponse();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }

            var account = accountManager.GetById(id);

            if (account == null)
            {
                response.Message(ResponseCodes.ACCOUNT_DOES_NOT_EXIST);
                return NotFound(response);
            }

            var allowRequest = authorizationService.AuthorizeAsync(this.User, account, new GeneralAuthorization(OperationType.Update));
            if (!allowRequest.IsCompletedSuccessfully)
            {
                return Forbid();
            }

            if (request.Address != null)
            {
                account.Address = request.Address;
            }
            if (request.City != null)
            {
                account.City = request.City;
            }
            if (request.Phone != null)
            {
                account.Phone = request.Phone;
            }
            if (request.PostalCode != null)
            {
                account.PostalCode = request.PostalCode;
            }
            if (request.ImageFile != null)
            {
                if (account.ProfileImage != null) imageManager.DeleteFile(account.ProfileImage);

                var image = new Image()
                {
                    ImageName = request.ImageFile.FileName,
                    ImageLocation = "#PROFILE_PICTURE",
                    Role = ImageRole.Profile,
                    Title = request.ImageFile.FileName.Split('.')[0]
                };

                var result = imageManager.UploadFile(image, request.ImageFile);

                if (!result.Succeeded)
                {
                    var dict = result.TransformToDict();
                    response.Errors = dict.GetModelError(dynamicTypeFactory);
                    return BadRequest(response);
                }

                account.ImageId = image.Id;
            }
            if (account.AccountType == AccountType.Restaurant)
            {
                if (request.Restaurant != null && request.Restaurant.Name != null)
                {
                    account.Restaurant.Name = request.Restaurant.Name;
                }
                if (request.Restaurant != null && request.Restaurant.Description != null)
                {
                    account.Restaurant.Description = request.Restaurant.Description;
                }
            }
            else
            {
                if (request.User != null && request.User.FirstName != null)
                {
                    account.User.FirstName = request.User.FirstName;
                }
                if (request.User != null && request.User.LastName != null)
                {
                    account.User.LastName = request.User.LastName;
                }
                if (request.User != null && request.User.DateOfBirth != null)
                {
                    account.User.DateOfBirth = request.User.DateOfBirth;
                }
            }

            unitOfWork.SaveChanges();
            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;
            return Ok(response);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            var response = new ApiResponse();
            var dict = new Dictionary<string, List<string>>();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.GetErrors(dynamicTypeFactory);
                return BadRequest(response);
            }
            
            var userId = this.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value;
            var account = accountManager.GetById(Convert.ToInt32(userId));

            if(account == null)
            {
                response.Message = ResponseCodes.ACCOUNT_DOES_NOT_EXIST;
                return NotFound(response);
            }

            var result = accountManager.ChangePassword(account, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                dict.Add(nameof(request.OldPassword), new List<string>() { ResponseCodes.PASSWORD_NOT_MATCHED });

                response.Errors = dict.GetModelError(dynamicTypeFactory);
                return BadRequest(response);
            }

            response.Message = ResponseCodes.SUCCESSFUL_REQUEST;
            return Ok(response);
        }
    }
}
