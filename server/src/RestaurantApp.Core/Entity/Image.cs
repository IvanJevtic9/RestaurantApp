namespace RestaurantApp.Core.Entity
{
    public enum ImageRole
    {
        Profile,
        Gallery,
        Menu,
        Dish
    }

    public class Image
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }
        public ImageRole Role { get; set; }
    }
}
