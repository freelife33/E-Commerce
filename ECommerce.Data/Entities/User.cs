using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
                     
        [Required]   
        public string PasswordHash { get; set; }
                     
        [StringLength(100)]
        public string FullName { get; set; }
                     
        [Required]   
        [EmailAddress]
        public string Email { get; set; }
                     
        [Phone]      
        public string PhoneNumber { get; set; }
                     
        public string Address { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        // Navigation property for user's orders
        public ICollection<Order> Orders { get; set; }

        public Cart Cart { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; }
        public ICollection<ProductReview> ProductReviews { get; set; }
        public ICollection<ProductRating> ProductRatings { get; set; }
        public ICollection<Log> Logs { get; set; }


    }
}
