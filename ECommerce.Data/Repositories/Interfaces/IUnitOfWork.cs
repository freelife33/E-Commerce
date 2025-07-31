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
        IProductImageRepository ProductImages { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        ICuponRepository Cupons { get; }
        IUserRepository Users { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        IWishlistRepository Wishlist { get; }
        IWishlistItemRepository WishlistItems { get; }
        IAddressRepository Address { get; }
        IPaymentRepository Payments { get; }
        IPaymentMethod PaymentMethods { get; }
        IBankAccount BankAccounts { get; }

        Task<int> ComplateAsync();
        void Dispose();
    }
}
