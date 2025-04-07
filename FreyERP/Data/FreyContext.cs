using FreyERP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Data
{
    public class FreyContext : DbContext
    {
        public DbSet<Customer> Cusutomers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=FreyERPDb; Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Optional: Seed-Daten
                new Customer { Id = 1, Vorname = "Max", Nachname = "Mustermann", Email = "max@freyerp.de", Phone = "0123456789" };
        }
    }
}
