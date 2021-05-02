using RestaurantApp.Core.Entity;
using RestaurantApp.Core.RepositoryInterface;

namespace RestaurantApp.Infrastructure.Data.Repository
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
