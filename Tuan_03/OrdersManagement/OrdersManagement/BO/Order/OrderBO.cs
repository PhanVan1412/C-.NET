namespace OrdersManagement.BO.OrderBO
{
    public class SearchParamBO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class OrderResponseBO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
    }
}
