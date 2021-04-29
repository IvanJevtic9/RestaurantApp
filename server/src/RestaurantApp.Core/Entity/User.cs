using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Core.Entity
{
    public class User
    {
        public int Id { get; }

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AccountType Type { get; } = AccountType.User;
        public DateTime? DateOfBirth { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
