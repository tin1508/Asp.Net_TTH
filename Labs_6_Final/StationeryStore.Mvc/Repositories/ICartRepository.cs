using StationeryStore.Mvc.Models;
using StationeryStore.MvC.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public interface ICartRepository : IRepository<Cart, int>
{
    Task<Cart?> GetByUserIdAsync(string userId);
    Task<Cart> CreateCartAsync(string userId);
    Task AddItemAsync(CartItem item);
    Task<CartItem?> GetItemAsync(int cartId, int stationeryId);
    Task UpdateItemAsync(CartItem item);
    Task RemoveItemAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
}