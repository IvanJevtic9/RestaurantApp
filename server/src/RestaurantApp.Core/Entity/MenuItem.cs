namespace RestaurantApp.Core.Entity
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemDescription { get; set; }
        public string Attributes { get; set; }
        public double Price { get; set; }

        public int? ItemImageId { get; set; }
        public virtual Image ItemImage { get; set; }

        public int MenuId { get; set; }
        public virtual RestaurantMenu Menu {get;set;}
    }
}
