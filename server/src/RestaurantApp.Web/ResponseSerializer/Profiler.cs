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
        }
    }
}
