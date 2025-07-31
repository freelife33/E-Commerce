namespace ECommerce.Web.Models.Order
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }

}
