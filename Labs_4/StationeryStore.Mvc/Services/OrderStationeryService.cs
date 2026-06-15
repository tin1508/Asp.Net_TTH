using Microsoft.Extensions.Options;
using StationeryStore.Mvc.Exception;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Options;

namespace StationeryStore.Mvc.Services;

public interface IOrderStationeryService
{
    Task<StationeryOrder> CreateNewOrder(OrderCreateViewModel model);
    Task<List<OrderListItemViewModel>> GetAllOrderListAsync();
}
public class OrderStationeryService : IOrderStationeryService
{
    private readonly IStationeryOrderRepository _stationeryOrderRepository;
    private readonly IStationeryRepository _stationeryRepository;
    private readonly AppSettings _settings;
    public OrderStationeryService(IStationeryOrderRepository stationeryOrderRepository, IStationeryRepository stationeryRepository, IOptions<AppSettings> options)
    {
        _stationeryOrderRepository = stationeryOrderRepository;
        _stationeryRepository = stationeryRepository;
        _settings = options.Value;
    }
    public async Task<StationeryOrder> CreateNewOrder(OrderCreateViewModel model)
    {
        var order = new StationeryOrder
        {
            CustomerName = model.CustomerName.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        foreach (var item in model.Items.Where(i => i.StationeryId > 0 && i.Quantity > 0))
        {
            var stationery = await _stationeryRepository.GetByIdAsync(item.StationeryId);
            if (stationery == null) throw new AppException(ErrorCode.NOT_EXISTED_STATIONERY);
            if(stationery.Quantity < item.Quantity)
            {
                throw new AppException(ErrorCode.NOT_ENOUGH_QUANTITY);
            }

            stationery.Quantity -= item.Quantity;

            order.OrderStationeries.Add(new OrderDetail
            {
                StationeryId = item.StationeryId,
                Quantity = item.Quantity,
                UnitPrice = stationery.Price,
                
            });
        }

        if (!order.OrderStationeries.Any())
        {
            throw new AppException(ErrorCode.AT_LEAST_STATIONERY_WHEN_ORDERING);
        }

        order.TotalAmount = order.OrderStationeries.Sum(od => od.SubTotal);


        await _stationeryOrderRepository.AddAsync(order);
        await _stationeryOrderRepository.SaveChangeAsync();
        return order;
    }
    public async Task<List<OrderListItemViewModel>> GetAllOrderListAsync()
    {
        var orders = await _stationeryOrderRepository.GetAllAsync();
        return orders.Select(o => new OrderListItemViewModel
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            CreatedAt = o.CreatedAt,
            TotalAmount = o.TotalAmount
        }).ToList();
    }

}