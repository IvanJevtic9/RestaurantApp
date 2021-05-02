using Microsoft.AspNetCore.Identity;
using RestaurantApp.Core.IdentityProvider;
using RestaurantApp.Core.Model;

namespace RestaurantApp.Core.Interface
{
    public interface IAccountManager<TAccount> where TAccount : class
    {
        public ILoggerAdapter<TAccount> Logger { get; }
        public IPasswordHasher<TAccount> PasswordHasher { get; }
        public IJwtProvider JwtProvider { get; }

        public TAccount GetByEmail(string email, string includeProperties = null);
        public TAccount GetById(int accountId, string includeProperties = null);
        public void Delete(TAccount account);
        public OperationResult CreateAccount(TAccount account, string password);
        public bool CheckPassword(TAccount account, string password);

    }
}
