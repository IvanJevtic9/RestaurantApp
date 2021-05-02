using RestaurantApp.Core.Entity;
using RestaurantApp.Core.RepositoryInterface;

namespace RestaurantApp.Infrastructure.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
