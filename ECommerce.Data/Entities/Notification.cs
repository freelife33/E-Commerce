using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
