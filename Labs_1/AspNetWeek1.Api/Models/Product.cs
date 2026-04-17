namespace AspNetWeek1.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
            public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public String CategoryName { get; set; }  = String.Empty;
    }
}