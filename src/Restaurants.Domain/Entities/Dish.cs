﻿namespace Restaurants.Domain.Entities
{
    public class Dish
    {
       public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Decimal? Price { get; set; }
        public int? KiloCarlories { get; set; }
        public int RestaurantId { get; set; }
    }
}