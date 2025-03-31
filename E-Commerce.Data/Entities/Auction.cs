using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Entities
{
    public class Auction
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public int? WinningBidId { get; set; }
        public Bid? WinningBid { get; set; }
    }
}
