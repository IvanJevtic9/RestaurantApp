using FluentValidation;
using RestaurantApp.Infrastructure;
using RestaurantApp.Web.WebModel;
using System.Text.RegularExpressions;

namespace RestaurantApp.Web.Validator
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Email).NotEmpty().WithMessage(l => ResponseCodes.RequiredField(nameof(l.Email)));
            RuleFor(l => l.Password).NotEmpty().WithMessage(l => ResponseCodes.RequiredField(nameof(l.Password)));

            RuleFor(l => l.Email)
                .Matches(Constants.EMAIL_REGEX, RegexOptions.IgnoreCase)
                .When(l => !string.IsNullOrEmpty(l.Email))
                .WithMessage(l => ResponseCodes.InvalidValue(nameof(l.Email)));
        }
    }
}
