using Domain;
using Domain.Menu;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetOrder
    {
        private const string drinkFilter = "Water";
        private const string dinnerDrinkFilter = "Wine";

        public class Query : IRequest<string>
        {
            public string MealType { get; set; }

            public List<int> Orders { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            private bool NotEqualZero(List<int> ints)
            {
                return ints.All(i => i != 0);
            }

            private bool NotGreaterThanFour(List<int> ints)
            {
                return ints.All(i => i < 5);
            }

            private bool MustHaveAMain(List<int> ints)
            {
                return ints.Contains(1);
            }

            private bool MustHaveASide(List<int> ints)
            {
                return ints.Contains(2);
            }

            private bool DessertWithDinnerOnlyAndRequired(string mealType, List<int> ints)
            {
                Enum.TryParse(mealType, out Meal meal);
                if (meal == Meal.Dinner)
                {
                    return ints.Contains(4);
                } 
                else
                {
                    return !ints.Contains(4);
                }
            }

  
            private bool CannotOrderMultipleMains(List<int> ints)
            {
                return ints.Where(x => x == 1).Count() < 2;
            }

            private bool CannotOrderMultipleSides(string mealType, List<int> ints)
            {
                Enum.TryParse(mealType, out Meal meal);
                if (meal == Meal.Lunch)
                {
                    return ints.Where(x => x == 2).Count() >= 1;
                }
                else
                {
                    return ints.Where(x => x == 2).Count() < 2;
                }
            }

            private bool CannotOrderMultipleDrinks(string mealType, List<int> ints)
            {

                Enum.TryParse(mealType, out Meal meal);
                if (meal == Meal.Breakfast)
                {
                    return ints.Where(x => x == 3).Count() >= 0;
                }
                else 
                {
                    return ints.Where(x => x == 3).Count() < 2;
                }
            }

            private bool CannotOrderMultipleDesserts(List<int> ints)
            {
                return ints.Where(x => x == 4).Count() < 2;
            }


            public QueryValidator()
            {
                RuleFor(x => x.MealType)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("Meal Type must be specified")
                    .IsEnumName(typeof(Meal), caseSensitive: true).WithMessage("Meal Type needs to be case-sensitve Breakfast, Lunch, or Dinner");
                RuleFor(x => x.Orders)
                    //Stop on first failure to avoid exception in method with null value
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Order List Required")
                    .Must(NotEqualZero).WithMessage("Valid Orders Required")
                    .Must(NotGreaterThanFour).WithMessage("Valid Orders Required")
                    .Must(MustHaveAMain).WithMessage("Main Dish Required")
                    .Must(MustHaveASide).WithMessage("Side Dish Required")
                    .Must(CannotOrderMultipleMains).WithMessage("Cannot order multiple Main Dishes")
                    .Must(CannotOrderMultipleDesserts).WithMessage("Cannot order multiple Desserts");
                RuleFor(x => x)
                    .Must(m => DessertWithDinnerOnlyAndRequired(m.MealType, m.Orders)).WithMessage("Dessert can only be ordered with Dinner and is required of a Dinner order")
                    .Must(m => CannotOrderMultipleSides(m.MealType, m.Orders)).WithMessage("Only Lunch meals can order multiple sides")
                    .Must(m => CannotOrderMultipleDrinks(m.MealType, m.Orders)).WithMessage("Only Breakfast meals can order multiple drinks");
            }
        }

        public class Handler : IRequestHandler<Query, string>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                Order order = new Order();
                
                var mealType = (Meal) Enum.Parse(typeof(Meal), request.MealType);

                List<MainDish> mainDishes = await _context.MainDishes.Where(m => m.MealType == mealType).ToListAsync();
                List<SideDish> sideDishes = await _context.SidesDishes.Where(m => m.MealType == mealType).ToListAsync();
                List<Drink> drinks = await _context.Drinks.Where(m => m.MealType == mealType).ToListAsync();
                Dessert dessert = await _context.Desserts.Where(m => m.MealType == mealType).FirstOrDefaultAsync();

                order.MealType = mealType;
                order.Mains = mainDishes;

                if (mealType == Meal.Breakfast)
                {
                    order.Sides = sideDishes;
                    order.Drinks = new List<Drink>();

                    if (request.Orders.Contains(3))
                    {
                        var occurance = request.Orders.Where(x => x == 3).Count();
                        for (int i=0; i < occurance; i++)
                        {
                            order.Drinks.Add(drinks.Where(d => d.Name != drinkFilter).FirstOrDefault());
                        }
                    }
                    else
                    {
                        order.Drinks.Add(drinks.Where(d => d.Name == drinkFilter).FirstOrDefault());
                    }

                    order.Dessert = null;
                }

                if (mealType == Meal.Lunch)
                {
                    order.Sides = new List<SideDish>();
                    order.Drinks = new List<Drink>();

                    if (request.Orders.Contains(2))
                    {
                        var occurance = request.Orders.Where(x => x == 2).Count();
                        for (int i = 0; i < occurance; i++)
                        {
                            order.Sides.Add(sideDishes.FirstOrDefault());
                        }
                    }

                    if (request.Orders.Contains(3))
                    {
                        order.Drinks.Add(drinks.Where(d => d.Name != drinkFilter).FirstOrDefault());
                    }
                    else
                    {
                        order.Drinks.Add(drinks.Where(d => d.Name == drinkFilter).FirstOrDefault());
                    }
                    order.Dessert = null;
                }

                if (mealType == Meal.Dinner)
                {
                    order.Sides = sideDishes;
                    order.Drinks = new List<Drink>();

                    if (request.Orders.Contains(3))
                    {
                        order.Drinks.Add(drinks.Where(d => d.Name != drinkFilter).FirstOrDefault());
                        order.Drinks.Add(drinks.Where(d => d.Name == drinkFilter).FirstOrDefault());
                    }
                    else
                    {
                        order.Drinks.Add(drinks.Where(d => d.Name == drinkFilter).FirstOrDefault());
                    }

                    order.Dessert = dessert;
                }

                if (order != null)
                {
                    string mainName, sideName, drinkName, dessertName;

                    if (order.Mains.Count > 1)
                    {
                        mainName = order.Mains.FirstOrDefault().Name + "(" + order.Mains.Count + ")";
                    }
                    else
                    {
                        mainName = order.Mains.FirstOrDefault().Name;
                    }

                    if (order.Sides.Count > 1)
                    {
                        sideName = order.Sides.FirstOrDefault().Name + "(" + order.Sides.Count + ")";
                    }
                    else
                    {
                        sideName = order.Sides.FirstOrDefault().Name;
                    }

                    if (order.Drinks.Count > 1)
                    {
                        if (order.Drinks.Where(d => d.Name == dinnerDrinkFilter).Count() > 1)
                        {
                           drinkName = dinnerDrinkFilter + "(" + order.Drinks.Where(d => d.Name == dinnerDrinkFilter).Count() + "), " + drinkFilter;
                        }
                        else if (order.Drinks.Where(d => d.Name == dinnerDrinkFilter).Count() == 1)
                        {
                           drinkName = dinnerDrinkFilter + ", " + drinkFilter;
                        }
                        else
                        {
                            drinkName = order.Drinks.FirstOrDefault().Name + "(" + order.Drinks.Count + ")";
                        }
                    }
                    else
                    {
                        drinkName= order.Drinks.FirstOrDefault().Name;
                    }

                    if (order.MealType == Meal.Dinner)
                    {
                        dessertName = order.Dessert.Name;
                        return (mainName + ", " + sideName + ", " + drinkName + ", " + dessertName);
                    }
                    else
                    {
                        return (mainName + ", " + sideName + ", " + drinkName);
                    }
                }

                throw new Exception("Problem returning data");
            }
        }
    }
}
