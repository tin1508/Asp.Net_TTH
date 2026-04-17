namespace AspNetWeek1.Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}