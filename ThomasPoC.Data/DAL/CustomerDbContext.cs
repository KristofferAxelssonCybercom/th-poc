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
        //private readonly IMongoDatabase _database = null;

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options): base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public CustomerDbContext(IOptions<Settings> settings)
        {
            //var client = new MongoClient(settings.Value.ConnectionString);
            //if (client != null)
            //    _database = client.GetDatabase(settings.Value.Database);
            //return null;
        }

    }
}
