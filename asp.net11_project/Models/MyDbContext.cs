using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;//sử dụng để đọc file appsetting.json
using System.IO;//thao tác với các file


namespace asp.net11_project.Models
{
    public class MyDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string strDbConnectString = configuration.GetConnectionString("DBConnectString").ToString();
            optionBuilder.UseSqlServer(strDbConnectString);
        }
        public DbSet<UsersRecord> Users { get; set; }
        public DbSet<CategoriesRecord> Categories { get; set; }
        public DbSet<ProductsRecord> Products { get; set; }
        public DbSet<RatingRecord> Rating { get; set; }
        public DbSet<CustomersRecord> Customers { get; set; }
        public DbSet<OrdersRecord> Orders { get; set; }
        public DbSet<OrdersDetailRecord> OrdersDetail { get; set; }
    }
}
