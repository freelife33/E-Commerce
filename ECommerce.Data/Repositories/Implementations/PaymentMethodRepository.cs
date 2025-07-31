using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Implementations
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethod
    {
        public PaymentMethodRepository(AppDbContext context) : base(context)
        {
        }
    }
}
