namespace AspNetWeek1.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}