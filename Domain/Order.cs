using Domain.Menu;
using System.Collections.Generic;

namespace Domain
{
    public class Order
    {

        public Meal MealType { get; set; }

        public List<MainDish> Mains { get; set; }

        public List<SideDish> Sides { get; set; }

        public List<Drink> Drinks { get; set; }
        
        public Dessert Dessert { get; set; }
        
    }
}
