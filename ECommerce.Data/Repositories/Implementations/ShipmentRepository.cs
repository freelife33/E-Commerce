using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(AppDbContext context) : base(context) { }
    }
}