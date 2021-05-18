using System;

namespace RestaurantApp.Core.RepositoryInterface
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Account { get; }
        IUserRepository User { get; }
        IRestaurantRepository Restaurant { get; }
        IImageRepository Image { get; }

        void SaveChanges();
    }
}
