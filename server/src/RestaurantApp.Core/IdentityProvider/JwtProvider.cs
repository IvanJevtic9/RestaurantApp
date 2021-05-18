using Microsoft.IdentityModel.Tokens;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Setting;
using RestaurantApp.Core.WebModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantApp.Core.IdentityProvider
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings jwtSettings;
        private readonly AppSettings appSettings;
        public JwtProvider(JwtSettings jwtSettings, AppSettings appSettings)
        {
            this.jwtSettings = jwtSettings;
            this.appSettings = appSettings;
        }

        public JwtResponse GetJwtToken(Account account)
        {
            var claim = new List<Claim>
            {
                new Claim("email", account.Email),
                new Claim("id", account.Id.ToString()),
                new Claim("imageId", account.ImageId.ToString()),
                new Claim("imageName", account.ProfileImage.ImangeName),
                new Claim("imageUrl", account.ProfileImage.Url),
                new Claim("accountType", account.AccountType.ToString()),
                new Claim("address", account.Address),
                new Claim("city", account.City),
                new Claim("postalCode", account.PostalCode),
                new Claim("phone", account.Phone)
            };

            if (account.AccountType == AccountType.Restaurant)
            {
                claim.Add(new Claim("name", account.Restaurant.Name));
                claim.Add(new Claim("description", account.Restaurant.Description));
            }
            else
            {
                claim.Add(new Claim("firstName", account.User.FirstName));
                claim.Add(new Claim("lastName", account.User.LastName));
                if (account.User.DateOfBirth != null)
                {
                    claim.Add(new Claim("dateOfBirth", account.User.DateOfBirth?.ToString(appSettings.DateFormat)));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.Now.AddMinutes(jwtSettings.JwtExpireMinutes);

            var token = new JwtSecurityToken(
                    claims: claim,
                    expires: expireDate,
                    signingCredentials: credentials
                );

            return new JwtResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
