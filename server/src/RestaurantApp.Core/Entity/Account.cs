namespace RestaurantApp.Core.Entity
{
    /// <summary>
    /// Account types in application
    /// </summary>
    public enum AccountType
    {
        Restaurant,
        User
    }

    /// <summary>
    /// Account enitity
    /// </summary>
    public class Account
    {
        public int Id { get; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public AccountType AccountType { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }
    }
}
