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
            ProductImages = new ProductImageRepository(_context);
            Categories = new CategoryRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            Cupons = new CuponRepository(_context);
            Users = new UserRepository(_context);
            Carts = new CartRepository(_context);
            CartItems = new CartItemRepository(_context);
            Wishlist = new WishlistRepository(_context);
            WishlistItems = new WishlistItemRepository(_context);
            Address = new AddressRepository(_context);
            Payments = new PaymentRepository(_context);
            PaymentMethods = new PaymentMethodRepository(_context);
            BankAccounts = new BankAccountRepository(_context);
            SystemSettings = new SystemSettingsRepository(_context);
            ContactSettings = new ContactSettingRepository(_context);
            ContactMessages = new ContactMessageRepository(_context);
            SocialLinks = new SocialLinkRepository(_context);
            CustomOrderRequests = new CustomOrderRequestRepository(_context);
        }

        public IProductRepository Products { get; private set; }

        public IProductImageRepository ProductImages { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public IOrderDetailRepository OrderDetails { get; private set; }

        public ICuponRepository Cupons { get; private set; }

        public IUserRepository Users { get; private set; }

        public ICartRepository Carts { get; private set; }

        public ICartItemRepository CartItems { get; private set; }

        public IWishlistRepository Wishlist { get; private set; }

        public IWishlistItemRepository WishlistItems { get; private set; }

        public IAddressRepository Address { get; private set; }

        public IPaymentRepository Payments { get; private set; }

        public IPaymentMethod PaymentMethods { get; private set; }

        public IBankAccount BankAccounts { get; private set; }

        public ISystemSettingsRepository SystemSettings { get; private set; }

        public IContactSettingRepository ContactSettings{ get; private set; }

        public IContactMessageRepository ContactMessages { get; private set; }

        public ISocialLinkRepository SocialLinks { get; private set; }

        public ICustomOrderRequestRepository CustomOrderRequests { get; private set; }

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
