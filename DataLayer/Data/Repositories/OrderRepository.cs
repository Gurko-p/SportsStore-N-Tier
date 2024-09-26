using DataLayer.Data.Contexts;
using DataLayer.Data.Interfaces;
using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data.Repositories;

public class OrderRepository(ApplicationDbContext context) : IRepository<Order>
{
    public async Task<List<Order>> GetAllAsync()
    {
        return await context.Orders.ToListAsync();
    }

    public IQueryable<Order> Query()
    {
        return context.Orders.AsQueryable();
    }

    public async Task<Order?> FirstOrDefaultAsync(int id)
    {
        return await context.Orders.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Order item)
    {
        await context.Orders.AddAsync(item);
        await SaveChangesAsync();
    }

    public async Task<Order> CreateWithReturnCreatedAsync(Order item)
    {
        await context.Orders.AddAsync(item);
        await SaveChangesAsync();
        return item;
    }

    public async Task CreateRangeAsync(IEnumerable<Order> items)
    {
        await context.Orders.AddRangeAsync(items);
        await SaveChangesAsync();
    }


    public async Task UpdateAsync(Order item)
    {
        context.Orders.Update(item);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order != null)
            context.Orders.Remove(order);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}