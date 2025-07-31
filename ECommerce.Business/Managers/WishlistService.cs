using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishlistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Wishlist> GetOrCreateWishlistAsync(int userId)
        {
            var wishlist = await _unitOfWork.Wishlist
    .GetFirstOrDefaultAsync(
        w => w.UserId == userId,
        include: q => q.Include(w => w.WishlistItems)
                       .ThenInclude(wi => wi.Product)
                       .ThenInclude(p => p.Images)
                       .Include(w => w.WishlistItems)
                       .ThenInclude(wi => wi.Product)
                       .ThenInclude(p => p.Stock)
    );

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.Wishlist.AddAsync(wishlist);
                await _unitOfWork.ComplateAsync();
            }

            return wishlist;
        }

        public async Task<List<WishlistItem>> GetWishlistItemsAsync(int userId)
        {
            var wishlist = await GetOrCreateWishlistAsync(userId);
            return wishlist.WishlistItems.ToList();
        }

        public async Task<bool> AddToWishlistAsync(int userId, int productId)
        {
            var wishlist = await GetOrCreateWishlistAsync(userId);

            if (wishlist.WishlistItems.Any(wi => wi.ProductId == productId))
                return false; // Zaten ekli

            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = productId,
                AddedAt = DateTime.Now
            };

            await _unitOfWork.WishlistItems.AddAsync(wishlistItem);
            await _unitOfWork.ComplateAsync();
            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await GetOrCreateWishlistAsync(userId);
            var item = wishlist.WishlistItems.FirstOrDefault(wi => wi.ProductId == productId);

            if (item == null)
                return false;

            _unitOfWork.WishlistItems.Remove(item);
            await _unitOfWork.ComplateAsync();
            return true;
        }

        public async Task<bool> IsInWishlistAsync(int userId, int productId)
        {
            var wishlist = await GetOrCreateWishlistAsync(userId);
            return wishlist.WishlistItems.Any(wi => wi.ProductId == productId);
        }
    }

}
