using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Core.Entity
{
    public class Restaurant
    {
        public int Id { get; }

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public string Name { get; set; }
        public AccountType Type { get; } = AccountType.Restaurant;
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
