using AspNetWeek1.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek1.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }
    //endpoint lay tat ca don hang
    [HttpGet("orders")]
    public IActionResult GetAllOrders() 
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }
    //endpoint lay tong so tien cua don hang dua vao id don hang
    [HttpGet("totalamount/{orderId}")]
    public IActionResult GetTotalAmountByOrderId(int orderId)
    {
        try
        {
            var totalAmount = _orderService.GetTotalAmountByOrderId(orderId);
            return Ok("Total Amount: " + totalAmount);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}