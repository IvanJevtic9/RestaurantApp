using RestaurantApp.Core.Entity;
using RestaurantApp.Core.WebModels;

namespace RestaurantApp.Core.IdentityProvider
{
    public interface IJwtProvider
    {
        JwtResponse GetJwtToken(Account account);
    }
}
