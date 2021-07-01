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
            RuleFor(m => m.ManuBanner).NotEmpty().WithMessage(a => ResponseCodes.RequiredField(nameof(a.ManuBanner)));
        }
    }
}
