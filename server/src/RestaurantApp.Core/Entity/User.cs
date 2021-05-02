using System;

namespace RestaurantApp.Core.Entity
{
    /// <summary>
    /// User enitity
    /// </summary>
    public class User
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public AccountType Type { get; } = AccountType.User;

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
