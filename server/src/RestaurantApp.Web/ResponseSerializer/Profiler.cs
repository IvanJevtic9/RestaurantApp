using AutoMapper;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.RepositoryInterface;

namespace RestaurantApp.Web.ResponseSerializer
{
    public class Profiler : Profile
    {
        public Profiler(IUnitOfWork unitOfWork)
        {
            CreateMap<RestaurantMenu, MenuSerializer>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(model => model.MenuBanner.Url));
            CreateMap<MenuItem, MenuItemSerializer>()
                .ForMember(x => x.Description, opt => opt.MapFrom(model => model.ItemDescription))
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(model => model.ItemImage.Url));
            CreateMap<GalleryImage, GalleryImageSerializer>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(model => model.Image.Url));

            CreateMap<Restaurant, RestaurantGetSerializer>()
                .ForMember(x => x.MenuCategories, opt => opt.MapFrom(model => model.RestaurantMenus));
            CreateMap<RestaurantMenu, MenuListSerializer>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(model => model.MenuBanner.Url))
                .ForMember(x => x.MenuItems, opt => opt.MapFrom(model => model.Items));

            CreateMap<Restaurant, RestaurantListSerializer>()
                .ForMember(x => x.ProfileUrl, opt => opt.MapFrom(model => model.Account.ProfileImage.Url))
                .ForMember(x => x.City, opt => opt.MapFrom(model => model.Account.City))
                .ForMember(x => x.Address, opt => opt.MapFrom(model => model.Account.Address))
                .ForMember(x => x.PostalCode, opt => opt.MapFrom(model => model.Account.PostalCode))
                .ForMember(x => x.Phone, opt => opt.MapFrom(model => model.Account.Phone));

            CreateMap<PaymentOrder, PaymentOrderSerializer>()
                .ForMember(x => x.Restaurant, opt => opt.MapFrom(model => model.Restaurant))
                .ForMember(x => x.User, opt => opt.MapFrom(model => model.User));
            CreateMap<Restaurant, RestaurantDet>()
                .ForMember(x => x.ProfileUrl, opt => opt.MapFrom(model => model.Account.ProfileImage.Url))
                .ForMember(x => x.Address, opt => opt.MapFrom(model => model.Account.Address))
                .ForMember(x => x.Phone, opt => opt.MapFrom(model => model.Account.Phone))
                .ForMember(x => x.City, opt => opt.MapFrom(model => model.Account.City));
            CreateMap<User, UserDet>()
                .ForMember(x => x.ProfileUrl, opt => opt.MapFrom(model => model.Account.ProfileImage.Url))
                .ForMember(x => x.Address, opt => opt.MapFrom(model => model.Account.Address))
                .ForMember(x => x.Phone, opt => opt.MapFrom(model => model.Account.Phone))
                .ForMember(x => x.City, opt => opt.MapFrom(model => model.Account.City));
        }
    }
}
