
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public class CartRepository : Repository<Cart, int>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context){}
    public async Task<Cart?> GetByUserIdAsync(string userId)
    {
        return await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Stationery).FirstOrDefaultAsync(c => c.UserId == userId);
    } 
    public async Task<Cart> CreateCartAsync(string userId)
    {
        var cart = new Cart {UserId = userId};
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }
    public async Task AddItemAsync(CartItem item)
    {
        _context.CartItems.Add(item);
        await _context.SaveChangesAsync();
    }
    public async Task<CartItem?> GetItemAsync(int cartId, int stationeryId)
    {
        return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.StationeryId == stationeryId);
    }
    public async Task UpdateItemAsync(CartItem item)
    {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveItemAsync(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        if(item is not null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
    public async Task ClearCartAsync(int cartId)
    {
        var items = _context.CartItems.Where(ci => ci.CartId == cartId);
        if(items is not null)
        {
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
    
}