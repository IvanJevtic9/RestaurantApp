using FluentValidation;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.RepositoryInterface;
using RestaurantApp.Infrastructure;
using RestaurantApp.Web.WebModel;
using System;
using System.Text.RegularExpressions;

namespace RestaurantApp.Web.Validator
{
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        public AccountValidator(IUnitOfWork unitOfWork)
        {
            /*Required fields*/
            RuleFor(a => a.Email).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Email)));
            RuleFor(a => a.Password).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Password)));
            RuleFor(a => a.City).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.City)));
            RuleFor(a => a.Address).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Address)));
            RuleFor(a => a.PostalCode).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.PostalCode)));
            RuleFor(a => a.AccountType).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.AccountType)));

            /* Length rules */
            RuleFor(a => a.Password).MinimumLength(8).When(a => !string.IsNullOrEmpty(a.Password)).WithMessage(a => ResponseCodes.LengthError(nameof(a.Password), true, 8));

            RuleFor(a => a.Email).MaximumLength(254).When(a => !string.IsNullOrEmpty(a.Email)).WithMessage(a => ResponseCodes.LengthError(nameof(a.Email), false, 254));
            RuleFor(a => a.City).MaximumLength(189).When(a => !string.IsNullOrEmpty(a.City)).WithMessage(a => ResponseCodes.LengthError(nameof(a.City), false, 189));
            RuleFor(a => a.Address).MaximumLength(254).When(a => !string.IsNullOrEmpty(a.Address)).WithMessage(a => ResponseCodes.LengthError(nameof(a.Address), false, 254));
            RuleFor(a => a.PostalCode).MaximumLength(10).When(a => !string.IsNullOrEmpty(a.PostalCode)).WithMessage(a => ResponseCodes.LengthError(nameof(a.PostalCode), false, 10));
            RuleFor(a => a.Phone).MaximumLength(15).When(a => !string.IsNullOrEmpty(a.Phone)).WithMessage(a => ResponseCodes.LengthError(nameof(a.Phone), false, 15));

            RuleFor(a => a.ConfirmPassword).Equal(a => a.Password).WithMessage(ResponseCodes.MUST_BE_EQUAL_PASSWORDS);

            /* Regex rules */
            RuleFor(a => a.Email)
                .Matches(Constants.EMAIL_REGEX, RegexOptions.IgnoreCase)
                .When(a => !string.IsNullOrEmpty(a.Email))
                .WithMessage(a => ResponseCodes.InvalidValue(nameof(a.Email)));
            RuleFor(a => a.Password)
                .Matches(Constants.PASSWORD_REGEX)
                .When(a => !string.IsNullOrEmpty(a.Password))
                .WithMessage(a => $"{ResponseCodes.InvalidValue(nameof(a.Password))} {ResponseCodes.PASSWORD_RULES}");

            /*Custom rules*/
            RuleFor(a => a.AccountType).Custom((value, context) =>
            {
                var accountDto = context.InstanceToValidate;

                if (value != AccountType.User.ToString() && value != AccountType.Restaurant.ToString())
                {
                    context.AddFailure(context.PropertyName, ResponseCodes.INVALID_ACCOUNT_TYPE);
                }
                else if (value == AccountType.User.ToString())
                {
                    var user = accountDto.User;
                    if (user == null)
                    {
                        context.AddFailure(nameof(accountDto.User), ResponseCodes.RequiredField(nameof(accountDto.User)));
                    }
                    else
                    {
                        if (user.FirstName == null) context.AddFailure(nameof(user.FirstName), ResponseCodes.RequiredField(nameof(user.FirstName)));
                        else if(user.FirstName.Length < 2) context.AddFailure(nameof(user.FirstName), ResponseCodes.LengthError(nameof(user.FirstName), true, 2));
                        else if (!Regex.IsMatch(user.FirstName, Constants.NAME_REGEX)) context.AddFailure(nameof(user.FirstName), $"{ResponseCodes.InvalidValue(nameof(user.FirstName))}");

                        if (user.LastName == null) context.AddFailure(nameof(user.LastName), ResponseCodes.RequiredField(nameof(user.LastName)));
                        else if (user.LastName.Length < 2) context.AddFailure(nameof(user.LastName), ResponseCodes.LengthError(nameof(user.LastName), true, 2));
                        else if (!Regex.IsMatch(user.LastName, Constants.NAME_REGEX)) context.AddFailure(nameof(user.LastName), $"{ResponseCodes.InvalidValue(nameof(user.LastName))}");

                        if (DateTime.Now < user.DateOfBirth )
                        {
                            context.AddFailure(nameof(user.DateOfBirth), ResponseCodes.INVALID_DATE_OF_BIRTH);
                        }
                    }
                }
                else if(value == AccountType.Restaurant.ToString())
                {
                    var restaurant = accountDto.Restaurant;
                    if(restaurant == null)
                    {
                        context.AddFailure(nameof(accountDto.Restaurant), ResponseCodes.RequiredField(nameof(accountDto.Restaurant)));
                    }
                    else
                    {
                        if (restaurant.Name == null) context.AddFailure(nameof(restaurant.Name), ResponseCodes.RequiredField(nameof(restaurant.Name)));
                    }
                }
            });

            RuleFor(a => a.Email).Custom((value, context) =>
            {
                if (unitOfWork.Account.Any(a => a.Email == value))
                {
                    context.AddFailure(context.PropertyName, ResponseCodes.EMAIL_ALREADY_REGISTERED);
                }
            });
        }
    }
}
