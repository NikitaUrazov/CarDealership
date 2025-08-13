using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Data
{
    public class ApplicationContext : DbContext
    {
        #region Fields
        
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<CarConfiguration> CarConfigurations { get; set; }
        public DbSet<Order> Orders { get; set; }

        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CarDealership;Trusted_Connection=True;");
            }
        }
        #endregion
    }
}
