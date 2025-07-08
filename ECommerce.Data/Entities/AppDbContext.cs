using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }//24
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cupon> Cupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductRating> ProductRatings { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionImage> AuctionImages { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Stock> Stocks { get; set; }


    }

}
