using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Entities
{
    public class ReturnRequest
    {
        public int Id { get; set; }
        public string? Reason { get; set; }
        public DateTime RequestDate { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
