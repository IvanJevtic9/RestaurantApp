using System.Collections.Generic;

namespace RestaurantApp.Core.Entity
{
    public class RestaurantMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? MenuBannerId { get; set; }
        public virtual Image MenuBanner { get; set; }

        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<MenuItem> Items { get; set; }
    }
}
