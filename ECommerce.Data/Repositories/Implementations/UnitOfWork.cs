using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Data.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            Cupons = new CuponRepository(_context);
            Users = new UserRepository(_context);

        }

        public IProductRepository Products { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public IOrderDetailRepository OrderDetails { get; private set; }

        public ICuponRepository Cupons { get; private set; }

        public IUserRepository Users { get; private set; }

        public async Task<int> ComplateAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
