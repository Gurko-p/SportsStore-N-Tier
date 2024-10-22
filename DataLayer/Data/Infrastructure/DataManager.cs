using DataLayer.Data.Contexts;
using DataLayer.Data.Interfaces;
using DataLayer.Data.Models;

namespace DataLayer.Data.Infrastructure;

public class DataManager : IDisposable
{
    public readonly IRepository<Category> Categories;
    public readonly IRepository<Cart> Carts;
    public readonly IRepository<Order> Orders;
    public readonly IRepository<Product> Products;
    public readonly IRatingRepository Ratings;
    public readonly ApplicationDbContext ApplicationDbContext;

    public DataManager(
        IRepository<Category> categories,
        IRepository<Cart> orderItems,
        IRepository<Order> orders,
        IRepository<Product> products,
        IRatingRepository ratings,
        ApplicationDbContext applicationDbContext
        )
    {
        Categories = categories;
        Carts = orderItems;
        Orders = orders;
        Products = products;
        Ratings = ratings;
        ApplicationDbContext = applicationDbContext;
    }

    private bool _disposed;

    public virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                ApplicationDbContext.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}