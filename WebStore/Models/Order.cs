namespace WebStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public double TotalPrice { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}
