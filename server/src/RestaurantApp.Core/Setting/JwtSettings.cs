namespace RestaurantApp.Core.Setting
{
    public class JwtSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireMinutes { get; set; }
    }
}
