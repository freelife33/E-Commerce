using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        ICuponRepository Cupons { get; }
        IUserRepository Users { get; }
        Task<int> ComplateAsync();
        void Dispose();
    }
}
