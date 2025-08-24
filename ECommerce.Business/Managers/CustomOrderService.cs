using ECommerce.Business.Services;
using ECommerce.Common.Models;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.CustomOrder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class CustomOrderService : ICustomOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _email;

        public CustomOrderService(IUnitOfWork uow, IEmailService email)
        {
            _uow = uow;
            _email = email;
        }

        public async Task<int> CreateAsync(CreateCustomOrderRequestDto dto, IEnumerable<UploadedFileInfo> files)
        {
            var repo = _uow.CustomOrderRequests;
            var entity = new CustomOrderRequest
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                ProductType = dto.ProductType,
                Dimensions = dto.Dimensions,
                WoodType = dto.WoodType,
                Color = dto.Color,
                Quantity = dto.Quantity,
                Finish = dto.Finish,
                EngravingText = dto.EngravingText,
                BudgetMin = dto.BudgetMin,
                BudgetMax = dto.BudgetMax,
                DesiredDate = dto.DesiredDate,
                Address = dto.Address,
                AdditionalNote = dto.AdditionalNote,
                Status = CustomOrderStatus.New
            };

            foreach (var f in files ?? Enumerable.Empty<UploadedFileInfo>())
                entity.Attachments.Add(new CustomOrderAttachment
                {
                    FileName = f.FileName,
                    FilePath = f.SavedPath,
                    ContentType = f.ContentType,
                    FileSize = f.Size,
                });

            await repo.AddAsync(entity);
            await _uow.ComplateAsync();

            //// Basit teşekkür maili (opsiyonel)
            try
            {
                await _email.SendAsync(entity.Email,
                    "Talebiniz alındı",
                    $"Merhaba {entity.FullName}, özel sipariş talebiniz bize ulaştı. En kısa sürede dönüş yapacağız.").WaitAsync(TimeSpan.FromSeconds(15));
            }
            catch { /* opsiyonel */ }

            return entity.Id;
        }

        public async Task<PagedResult<CustomOrderRequest>> GetPagedAsync(int page, int pageSize, CustomOrderStatus? status = null, string? q = null)
        {
            var repo = _uow.CustomOrderRequests;
            IQueryable<CustomOrderRequest> query = repo.Query().Include(x => x.Attachments);

            if (status.HasValue) query = query.Where(x => x.Status == status.Value);
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(x => x.FullName.Contains(q) || x.Email.Contains(q) || x.ProductType!.Contains(q));

            var total = await query.CountAsync();
            var items = await query.OrderByDescending(x => x.CreatedAt)
                                   .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<CustomOrderRequest>
            {
                Items = items,
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public Task<CustomOrderRequest?> GetAsync(int id) =>
            _uow.CustomOrderRequests
                .Query().Include(x => x.Attachments)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAdminAsync(int id, UpdateCustomOrderAdminDto dto)
        {
            var repo = _uow.CustomOrderRequests;
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return;

            entity.Status = (CustomOrderStatus)dto.Status;
            entity.QuoteAmount = dto.QuoteAmount;
            entity.QuoteNote = dto.QuoteNote;
            entity.QuotedAt = dto.QuoteAmount.HasValue ? DateTime.UtcNow : entity.QuotedAt;
            entity.UpdatedAt = DateTime.UtcNow;

            repo.Update(entity);
            await _uow.ComplateAsync();

            // İstersen teklif e-postası gönder:
            // await _email.SendAsync(entity.Email, "Teklifiniz hazır", $"Fiyat: {dto.QuoteAmount:C0} ...");
        }

        public async Task DeleteAsync(int id)
        {
            var repo = _uow.CustomOrderRequests;
            var e = await repo.GetByIdAsync(id);
            if (e == null) return;
            repo.Remove(e);
            await _uow.ComplateAsync();
        }
    }

}
