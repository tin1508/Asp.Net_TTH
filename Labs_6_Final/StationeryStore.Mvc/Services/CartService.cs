using AutoMapper;
using StationeryStore.Mvc.Exception;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using System.Reflection.Metadata.Ecma335;

namespace StationeryStore.Mvc.Services;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync(string userId);
    Task AddToCartAsync(string userId, int stationeryId, int quantity);
    Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);
    Task RemoveItemAsync(string userId, int cartItemId);
    Task<string> CheckoutAsync(string userId);
}

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IStationeryRepository _stationeryRepository;
    private readonly IOrderStationeryService _orderService;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IStationeryRepository stationeryRepository,
        IOrderStationeryService orderService,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _stationeryRepository = stationeryRepository;
        _orderService = orderService;
        _mapper = mapper;
    }

    public async Task<CartViewModel> GetCartAsync(string userId)
    {   
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if(cart is null) return new CartViewModel();
        return _mapper.Map<CartViewModel>(cart);   
    }

    public async Task AddToCartAsync(string userId, int stationeryId, int quantity)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId) ?? await _cartRepository.CreateCartAsync(userId);
        var existingItem = await _cartRepository.GetItemAsync(cart.Id, stationeryId);
        if(existingItem is not null)
        {
            existingItem.Quantity += quantity;
            await _cartRepository.UpdateItemAsync(existingItem);
        }
        else
        {
            var stationery = await _stationeryRepository.GetByIdAsync(stationeryId);
            if(stationery is null) throw new AppException(ErrorCode.NOT_EXISTED_STATIONERY);
            await _cartRepository.AddItemAsync(new CartItem
            {
                CartId = cart.Id,
                StationeryId = stationery.Id,
                Quantity = quantity,
                UnitPriceSnapshot = stationery.Price
            });
        }
    }
    public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId) ?? throw new AppException(ErrorCode.NOT_EXISTED_CART);
        var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId) ?? throw new AppException(ErrorCode.EMPTY_CART_ITEM);
        item.Quantity = quantity;
        await _cartRepository.UpdateItemAsync(item);
    }
    public async Task RemoveItemAsync(string userId, int cartItemId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId) ?? throw new AppException(ErrorCode.NOT_EXISTED_CART);
        var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
        if(item is null) return;
        await _cartRepository.RemoveItemAsync(cartItemId);
    }
    public async Task<string> CheckoutAsync(string userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId) ?? throw new AppException(ErrorCode.NOT_EXISTED_CART);
        if (cart.Items is null || !cart.Items.Any())
        throw new AppException(ErrorCode.AT_LEAST_STATIONERY_WHEN_ORDERING);


        var savedOrder = await _orderService.CreateNewOrder(cart);
        await _cartRepository.ClearCartAsync(cart.Id);
        return savedOrder.Id;
    }
}