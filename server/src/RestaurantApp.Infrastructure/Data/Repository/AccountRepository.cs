using RestaurantApp.Core.Entity;
using RestaurantApp.Core.RepositoryInterface;

namespace RestaurantApp.Infrastructure.Data.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
