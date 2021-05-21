using RestaurantApp.Core.Entity;
using RestaurantApp.Core.RepositoryInterface;

namespace RestaurantApp.Infrastructure.Data.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext db) : base(db)
        { }
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        { }
    }
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext db) : base(db)
        { }
    }
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext db) : base(db)
        { }
    }
    public class GalleryImageRepository : Repository<GalleryImage>, IGalleryImageRepository
    {
        public GalleryImageRepository(ApplicationDbContext db) : base(db)
        { }
    }
    public class RestaurantMenuRepository : Repository<RestaurantMenu>, IRestaurantMenuRepository
    {
        public RestaurantMenuRepository(ApplicationDbContext db) : base(db)
        { }
    }
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
