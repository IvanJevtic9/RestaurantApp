using RestaurantApp.Core.RepositoryInterface;

namespace RestaurantApp.Infrastructure.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public IAccountRepository Account { get; }

        public IUserRepository User { get; }

        public IRestaurantRepository Restaurant { get; }

        public IImageRepository Image { get; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;

            Account = new AccountRepository(db);
            User = new UserRepository(db);
            Restaurant = new RestaurantRepository(db);
            Image = new ImageRepository(db);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
