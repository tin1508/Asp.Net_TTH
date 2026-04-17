using AspNetWeek1.Api.Models;
namespace AspNetWeek1.Api.Services;

public class OrderService
{
    private readonly List<Order> _orders = [
        new Order
        {
            Id = 1,
            OrderDate = DateTime.Now,
            Products = new List<Product>
            {
                new Product { Id = 1, Name = "Faber-Castell 9000 Graphite Pencil (HB)", Price = 80, StockQuantity = 120, CategoryName = "Writing Instruments" },
                new Product { Id = 3, Name = "Pastel Sticky Notes (3x3)", Price = 35, StockQuantity = 75, CategoryName = "Paper & Notebooks" }
            }
        },
        new Order
        {
            Id = 2,
            OrderDate = DateTime.Now,
            Products = new List<Product>
            {
                new Product { Id = 2, Name = "Zebra Mildliner Highlighter (Set of 5)", Price = 180, StockQuantity = 40, CategoryName = "Writing Instruments" }
            }
        }
    ];
    private void CalculateTotalAmount()
    {
        foreach(var order in _orders)
        {
            order.TotalAmount = order.Products.Sum(p => p.Price);
        }
    }
    public List<Order> GetAllOrders()
    {
        CalculateTotalAmount();
        return _orders;
    }
    public decimal GetTotalAmountByOrderId(int orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId) ?? throw new Exception($"Order with ID '{orderId}' not found.");
        return order.TotalAmount;
    }
}