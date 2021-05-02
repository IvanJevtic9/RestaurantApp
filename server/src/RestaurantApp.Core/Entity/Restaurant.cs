namespace RestaurantApp.Core.Entity
{
    /// <summary>
    /// Restaurant enitity
    /// </summary>
    public class Restaurant
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AccountType Type { get; } = AccountType.Restaurant;

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
