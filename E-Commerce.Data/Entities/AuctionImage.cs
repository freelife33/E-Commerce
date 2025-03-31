using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Entities
{
    public class AuctionImage
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsCover { get; set; }
        public int AuctionId { get; set; }
        public Auction? Auction { get; set; }
    }
}
