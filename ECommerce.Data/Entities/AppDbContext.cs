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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("a.digil");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Auction>()
                .HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.WinningBid)
                .WithMany()
                .HasForeignKey(a => a.WinningBidId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kullanıcı Rolleri için ilişki
            modelBuilder.Entity<UserRole>()
           .HasKey(ur => new { ur.UserId, ur.RoleId });


            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Product>()
        .HasIndex(p => p.Sku)
        .IsUnique();

            modelBuilder.Entity<ContactSetting>()
    .HasMany(c => c.SocialLinks)
    .WithOne(s => s.ContactSetting)
    .HasForeignKey(s => s.ContactSettingId)
    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CustomOrderAttachment>()
    .HasOne(a => a.Request)
    .WithMany(r => r.Attachments)
    .HasForeignKey(a => a.CustomOrderRequestId)
    .OnDelete(DeleteBehavior.Cascade);



        }
        public DbSet<Product> Products { get; set; }//24
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
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
        public DbSet<CustomOrderRequest> CustomOrderRequests { get; set; }
        public DbSet<CustomOrderAttachment> CustomOrderAttachments { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<SystemSettings> SystemSettingses { get; set; }

        public DbSet<ContactSetting> ContactSettings { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }



    }

}
