using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public enum CustomOrderStatus
    {
        New = 0,        // geldi
        Reviewed = 1,   // incelendi
        Quoted = 2,     // teklif gönderildi
        Approved = 3,   // müşteri onayladı
        InProgress = 4, // üretimde
        Completed = 5,  // bitti / teslim edildi
        Rejected = 6    // iptal/red
    }
}
