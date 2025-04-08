namespace WebStore.DTOs
{
    public class OrderRequestDto
    {
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
