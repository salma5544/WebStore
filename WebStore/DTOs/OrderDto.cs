namespace WebStore.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public int NumberOfProducts { get; set; }
    }
}
