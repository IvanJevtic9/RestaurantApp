using Microsoft.AspNetCore.Http;
using System;

namespace RestaurantApp.Web.WebModel
{
    public class AccountDto
    {
        public string Email { get; }
        public string Password { get; }
        public string ConfirmPassword { get; }
        public string Phone { get; }
        public string City { get; }
        public string Address { get; }
        public string PostalCode { get; }

        public IFormFile ImageFile { get; }
        public string AccountType { get; }

        public RestaurantDto Restaurant { get; }
        public UserDto User { get; }
    }

    public class AccountUpdateDto
    {
        public string Email { get; }
        public string Phone { get; }
        public string City { get; }
        public string Address { get; }
        public string PostalCode { get; }
        public IFormFile ImageFile { get; }

        public RestaurantDto Restaurant { get; }
        public UserDto User { get; }
    }

    public class RestaurantDto
    {
        public string Name { get; }
        public string Description { get; }
    }

    public class UserDto
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime? DateOfBirth { get; }
    }

    public class ChangePasswordDto
    {
        public string OldPassword { get; }
        public string NewPassword { get; }
        public string ConfirmPassword { get; }
    }
}
