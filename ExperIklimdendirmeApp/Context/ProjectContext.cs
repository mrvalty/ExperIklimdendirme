using ExperIklimdendirmeApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Context
{
    public class ProjectContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=MERVE;Database=exprDB1;Integrated Security=True");
            optionsBuilder.UseSqlServer("server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
            //optionsBuilder.UseSqlServer("server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QRCustomer> QRCustomers { get; set; }
        public DbSet<CalendarEvents> CalendarEvents { get; set; }


    }
}