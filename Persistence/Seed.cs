using Domain.Menu;
using System.Collections.Generic;
using System.Linq;

namespace Persistence
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (!context.MainDishes.Any())
            {
                var mainDishes = new List<MainDish>
                {
                    new MainDish {MealType=0, Name="Eggs"},
                    new MainDish {MealType=(Domain.Meal)1, Name="Salad"},
                    new MainDish {MealType=(Domain.Meal)2, Name="Steak"}
                };

                context.MainDishes.AddRange(mainDishes);
                context.SaveChanges();
            }

            if (!context.SidesDishes.Any())
            {
                var sideDishes = new List<SideDish>
                {
                    new SideDish {MealType=0, Name="Toast"},
                    new SideDish {MealType=(Domain.Meal)1, Name="Chips"},
                    new SideDish {MealType=(Domain.Meal)2, Name="Potatoes"},
                };

                context.SidesDishes.AddRange(sideDishes);
                context.SaveChanges();
            }


            if (!context.Drinks.Any())
            {
                var drinks = new List<Drink>
                {
                    new Drink {MealType=0, Name="Coffee"},
                    new Drink {MealType=0, Name="Water"},
                    new Drink {MealType=(Domain.Meal)1, Name="Soda"},
                    new Drink {MealType=(Domain.Meal)1, Name="Water"},
                    new Drink {MealType=(Domain.Meal)2, Name="Wine"},
                    new Drink {MealType=(Domain.Meal)2, Name="Water"},
                };
                context.Drinks.AddRange(drinks);
                context.SaveChanges();
            }

            if (!context.Desserts.Any())
            {
                var desserts = new List<Dessert>
                {
                    new Dessert {MealType=(Domain.Meal)2, Name="Cake"},
                };
                context.Desserts.AddRange(desserts);
                context.SaveChanges();
            }
        }
    }
}
