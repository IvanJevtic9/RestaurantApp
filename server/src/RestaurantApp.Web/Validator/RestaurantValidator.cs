using FluentValidation;
using RestaurantApp.Core;
using RestaurantApp.Core.RepositoryInterface;
using RestaurantApp.Web.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.Validator
{
    public class RestaurantValidator : AbstractValidator<RestaurantMenuDto>
    {
        public RestaurantValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Name)));
        }
    }

    public class MenuItemValidator : AbstractValidator<RestaurantMenuItemDto>
    {
        public MenuItemValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Name)));
            RuleFor(m => m.Description).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Description)));
            RuleFor(m => m.Price).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.Price)));
            RuleFor(m => m.MenuId).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.MenuId)));

            RuleFor(m => m.Price).Custom((value, context) =>
            {
                if (value <= 0)
                {
                    context.AddFailure(context.PropertyName, ResponseCodes.MUST_BE_POSITIVE);
                }
            });
        }
    }

    public class GalleryValidator : AbstractValidator<GalleryDto>
    {
        public GalleryValidator()
        {
            RuleFor(g => g.GalleryImages).Custom((value, context) =>
            {
                if (value.Count < 1)
                {
                    context.AddFailure(context.PropertyName, ResponseCodes.REQUIRED_LIST);
                }

                for (int l = 0; l < value.Count; l++)
                {
                    var extension = value[l].FileName.Split('.')[1].ToUpper();

                    if (!(Constants.AllowedImageExtensions.Contains(extension)))
                    {
                        context.AddFailure(context.PropertyName, ResponseCodes.INVALID_FILE_FORMAT + $"File {l} in list should be image format.");
                    }
                    break;
                }
            });
        }
    }

    public class PaymentOrderValidator : AbstractValidator<PaymentOrderDto>
    {
        public PaymentOrderValidator()
        {
            RuleFor(m => m.PaymentItems).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.PaymentItems)));
            RuleFor(m => m.TotalPrice).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.TotalPrice)));
            RuleFor(m => m.RestaurantId).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.RestaurantId)));
        }
    }

    public class PaymentOrderTransitionValidator : AbstractValidator<PaymentOrderTransitionDto>
    {
        public PaymentOrderTransitionValidator()
        {
            RuleFor(m => m.TransitionName).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.TransitionName)));
        }
    }
}
