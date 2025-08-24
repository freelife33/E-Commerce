using ECommerce.Common.Models;
using ECommerce.Data.Entities;
using ECommerce.DTOs.CustomOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface ICustomOrderService
    {
        Task<int> CreateAsync(CreateCustomOrderRequestDto dto, IEnumerable<UploadedFileInfo> files);
        Task<PagedResult<CustomOrderRequest>> GetPagedAsync(int page, int pageSize, CustomOrderStatus? status = null, string? q = null);
        Task<CustomOrderRequest?> GetAsync(int id);
        Task UpdateAdminAsync(int id, UpdateCustomOrderAdminDto dto);
        Task DeleteAsync(int id);
    }

}
