using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Dtos
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Decimal? Price { get; set; }
        public int? KiloCarlories { get; set; }

        public int RestaurantId { get; set; }
    }
}
