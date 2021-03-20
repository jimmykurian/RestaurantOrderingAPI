using Domain.Menu;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MainDish> MainDishes { get; set; }
        public DbSet<SideDish> SidesDishes { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Dessert> Desserts { get; set; }
    }
}
