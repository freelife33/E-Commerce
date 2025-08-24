using ECommerce.Business.Services;
using ECommerce.Common.Models;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Contact;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class ContactService:IContactService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _email;
        public ContactService(IUnitOfWork uow, IEmailService email)
        {
            _uow = uow;
            _email = email;
        }

        public async Task<ContactSetting> GetSettingsAsync()
        {
            var repo = _uow.ContactSettings; // ContactSetting repository
            var settings = await repo.Query()
                .Include(x => x.SocialLinks)
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new ContactSetting { CompanyName = "Şirket Adı", Email = "info@site.com" };
                await repo.AddAsync(settings);
                await _uow.ComplateAsync();
            }
            return settings;
        }

        public async Task UpdateSettingsAsync(UpdateContactSettingDto dto)
        {
            var repo = _uow.ContactSettings;
            var s = await GetSettingsAsync();

            s.CompanyName = dto.CompanyName;
            s.AddressLine = dto.AddressLine;
            s.City = dto.City;
            s.Country = dto.Country;
            s.Phone1 = dto.Phone1;
            s.Phone2 = dto.Phone2;
            s.Email = dto.Email;
            s.MapEmbedUrl = dto.MapEmbedUrl;
            s.WorkingHours = dto.WorkingHours;
            s.UpdatedAt = DateTime.UtcNow;

            // Social links: basit sync
            var linkRepo = _uow.SocialLinks;
            var existing = s.SocialLinks.ToList();
            // silinenleri kaldır
            foreach (var ex in existing)
                if (!dto.SocialLinks.Any(d => d.Id == ex.Id))
                    linkRepo.Remove(ex);

            // ekle/güncelle
            foreach (var d in dto.SocialLinks)
            {
                if (d.Id.HasValue)
                {
                    var ex = existing.First(x => x.Id == d.Id.Value);
                    ex.Name = d.Name; ex.Url = d.Url; ex.IconCss = d.IconCss;
                }
                else
                {
                    s.SocialLinks.Add(new SocialLink { Name = d.Name, Url = d.Url, IconCss = d.IconCss });
                }
            }

            repo.Update(s);
            await _uow.ComplateAsync();
        }

        public async Task<int> CreateMessageAsync(CreateContactMessageDto dto, string? ip, string? ua)
        {
            var msgRepo = _uow.ContactMessages;
            var entity = new ContactMessage
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Subject = dto.Subject,
                Message = dto.Message,
                IpAddress = ip,
                UserAgent = ua
            };
            await msgRepo.AddAsync(entity);
            await _uow.ComplateAsync();

            //// İstersen e-posta bildirimi
            try
            {
                await _email.SendAsync(
                    to: dto.Email,
                    subject: "Mesajınız alındı",
                    htmlBody: $"<p>Merhaba {dto.FullName},</p><p>Mesajınızı aldık. En kısa sürede size dönüş yapacağız.</p>"
                );
            }
            catch { /* e-posta opsiyonel; patlatma */ }

            return entity.Id;
        }

        public async Task<PagedResult<ContactMessage>> GetMessagesAsync(int page = 1, int pageSize = 20, string? q = null)
        {
            var repo = _uow.ContactMessages;
            var query = repo.Query();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(x => x.FullName.Contains(q) || x.Email.Contains(q) || x.Subject!.Contains(q) || x.Message.Contains(q));

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ContactMessage>(items, total, page, pageSize);
        }

        public async Task MarkReadAsync(int id, bool isRead = true)
        {
            var repo = _uow.ContactMessages;
            var msg = await repo.GetByIdAsync(id);
            if (msg == null) return;
            msg.IsRead = isRead;
            repo.Update(msg);
            await _uow.ComplateAsync();
        }

        public async Task DeleteMessageAsync(int id)
        {
            var repo = _uow.ContactMessages;
            var msg = await repo.GetByIdAsync(id);
            if (msg == null) return;
            repo.Remove(msg);
            await _uow.ComplateAsync();
        }
    }
}
