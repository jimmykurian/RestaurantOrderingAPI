using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Menu
{
    public class Dessert
    {
        [Key]
        public Guid Id { get; set; }

        public Meal MealType { get; set; }

        public string Name { get; set; }
    }
}
