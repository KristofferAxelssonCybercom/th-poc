using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThomasPoC.Data.DAL
{
    public class Settings
    {
        public string ConnectionString;
        public string Database;
    }

    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext()
        {
        }

        //public CustomerDbContext() : base() { }
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }


        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            for (int i = 0; i < 200; i++)
            {
                modelBuilder.Entity<Customer>().HasData(
                    new Faker<Customer>()
                        .RuleFor(o => o.id, f => f.IndexVariable++)
                        .RuleFor(o => o.title, f => f.Name.FirstName())
                        .RuleFor(o => o.userId, f => f.Random.Number(100, 200))
                        .RuleFor(o => o.body, f => f.Name.FirstName()).Generate());
            }
        }

    }
}
