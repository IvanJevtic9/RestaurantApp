using Microsoft.AspNetCore.Http;
using System;

namespace RestaurantApp.Web.WebModel
{
    public class AccountDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public IFormFile ImageFile { get; set; }
        public string AccountType { get; set; }

        public RestaurantDto Restaurant { get; set; }
        public UserDto User { get; set; }
    }

    public class RestaurantDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
