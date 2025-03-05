using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Entities
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public required DbSet<Product> Products { get; set; }
        public required DbSet<Category> Categories { get; set; }
        public required DbSet<User> Users { get; set; }
        public required DbSet<Cupon> Cupons { get; set; }
        public required DbSet<Order> Orders { get; set; }
        public required DbSet<OrderDetail> OrderDetails { get; set; }
    }
    
}
