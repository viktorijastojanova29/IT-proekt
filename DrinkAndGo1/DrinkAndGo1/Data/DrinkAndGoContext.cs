using DrinkAndGo1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DrinkAndGo1.Data
{
    public class DrinkAndGoContext : DbContext
    {
        public DrinkAndGoContext() : base("name=DrinkAndGo")
        {

        }

        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Category> Categorise { get; set; }
    }
}