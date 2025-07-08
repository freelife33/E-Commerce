using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context) { }
    }
}