using ECommerce.Common.Models;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IContactService
    {
        Task<ContactSetting> GetSettingsAsync();
        Task UpdateSettingsAsync(UpdateContactSettingDto dto);
        Task<int> CreateMessageAsync(CreateContactMessageDto dto, string? ip, string? ua);
        Task<PagedResult<ContactMessage>> GetMessagesAsync(int page = 1, int pageSize = 20, string? q = null);
        Task MarkReadAsync(int id, bool isRead = true);
        Task DeleteMessageAsync(int id);
    }
}
