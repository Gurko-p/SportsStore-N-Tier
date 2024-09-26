using DataLayer.Data.Contexts;
using DataLayer.Data.Interfaces;
using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data.Repositories;

public class CartRepository(ApplicationDbContext context) : IRepository<Cart>
{
    public async Task<List<Cart>> GetAllAsync()
    {
        return await context.Carts.ToListAsync();
    }

    public IQueryable<Cart> Query()
    {
        return context.Carts.AsQueryable();
    }

    public async Task<Cart?> FirstOrDefaultAsync(int id)
    {
        return await context.Carts.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Cart item)
    {
        await context.Carts.AddAsync(item);
        await SaveChangesAsync();
    }

    public async Task<Cart> CreateWithReturnCreatedAsync(Cart item)
    {
        await context.Carts.AddAsync(item);
        await SaveChangesAsync();
        return item;
    }

    public async Task CreateRangeAsync(IEnumerable<Cart> items)
    {
        await context.Carts.AddRangeAsync(items);
        await SaveChangesAsync();
    }


    public async Task UpdateAsync(Cart item)
    {
        context.Carts.Update(item);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var orderItem = await context.Carts.FindAsync(id);
        if (orderItem != null)
            context.Carts.Remove(orderItem);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}