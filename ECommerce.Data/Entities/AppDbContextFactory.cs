using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Connection stringini kendi ortamına göre düzenle!
            optionsBuilder.UseSqlServer("Server=DESKTOP-16394UD\\SQLEXPRESS;Database=ECommerce;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
