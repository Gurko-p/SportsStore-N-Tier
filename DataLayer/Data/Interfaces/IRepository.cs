namespace DataLayer.Data.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    IQueryable<T> Query();
    Task<T?> FirstOrDefaultAsync(int id);
    Task CreateAsync(T item);
    Task<T> CreateWithReturnCreatedAsync(T item);
    Task CreateRangeAsync(IEnumerable<T> items);
    Task UpdateAsync(T item);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}