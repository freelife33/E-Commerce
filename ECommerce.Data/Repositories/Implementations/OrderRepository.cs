using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Implementations
{
    public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
    {
        public async Task<bool> AnyAsync(Expression<Func<Order, bool>> predicate)
        {

            return await _context.Set<Order>().AnyAsync(predicate);
        }
    }
}
