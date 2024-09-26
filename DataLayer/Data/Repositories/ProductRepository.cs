using DataLayer.Data.Contexts;
using DataLayer.Data.Interfaces;
using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data.Repositories;

public class ProductRepository(ApplicationDbContext context) : IRepository<Product>
{
    public async Task<List<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public IQueryable<Product> Query()
    {
        return context.Products.AsQueryable();
    }

    public async Task<Product?> FirstOrDefaultAsync(int id)
    {
        return await context.Products.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product item)
    {
        await context.Products.AddAsync(item);
        await SaveChangesAsync();
    }

    public async Task<Product> CreateWithReturnCreatedAsync(Product item)
    {
        await context.Products.AddAsync(item);
        await SaveChangesAsync();
        return item;
    }

    public async Task CreateRangeAsync(IEnumerable<Product> items)
    {
        await context.Products.AddRangeAsync(items);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Product item)
    {
        context.Products.Update(item);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product != null)
            context.Products.Remove(product);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}