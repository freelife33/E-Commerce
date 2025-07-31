namespace ECommerce.Web.Models.Order
{
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Status { get; set; }
        public string AddressSummary { get; set; }
        public string CuponCode { get; set; }
    }

}
