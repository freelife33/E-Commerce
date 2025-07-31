namespace ECommerce.Web.Models.Order
{
    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }

    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; }

        public List<OrderItemViewModel> Items { get; set; }
    }

}
