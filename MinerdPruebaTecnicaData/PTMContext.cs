using Microsoft.EntityFrameworkCore;
using MinerdPruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinerdPruebaTecnica.Data
{
    public class PTMContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=PTMDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",b=>b.MigrationsAssembly("WebAppMinerd"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Data Seed
            modelBuilder.Entity<Account>()
                        .HasData
            (
                new Account
                {
                    ID = 1,
                    Nombre = "Jhon Prebisterio",
                    Email = "jprebisterio@gmail.com",
                    Password = "1234"
                }
            );
           
            #endregion
        }
    }
}
