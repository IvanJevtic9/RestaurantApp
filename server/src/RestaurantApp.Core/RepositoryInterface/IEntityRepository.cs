using RestaurantApp.Core.Entity;

namespace RestaurantApp.Core.RepositoryInterface
{
    public interface IAccountRepository : IRepository<Account>
    { }

    public interface IRestaurantRepository : IRepository<Restaurant>
    { }

    public interface IUserRepository : IRepository<User>
    { }

    public interface IImageRepository : IRepository<Image>
    { }

    public interface IRestaurantMenuRepository : IRepository<RestaurantMenu>
    { }

    public interface IMenuItemRepository : IRepository<MenuItem>
    { }

    public interface IGalleryImageRepository : IRepository<GalleryImage>
    { }
}
