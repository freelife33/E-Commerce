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

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

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


        public ICollection<UserRole> UserRoles { get; set; }=  new List<UserRole>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Cart Cart { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();
        public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public ICollection<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();
        public ICollection<Log> Logs { get; set; } = new List<Log>();


    }
}
