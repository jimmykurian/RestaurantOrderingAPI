using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Menu
{
    public class Drink
    {
        [Key]
        public Guid Id { get; set; }

        public Meal MealType {get; set; }

        public string Name { get; set; }
    }
}
