using DataLayer.Data.Contexts;
using DataLayer.Data.Interfaces;
using DataLayer.Data.Models;

namespace DataLayer.Data.Repositories;

public class RatingRepository(ApplicationDbContext context) : IRatingRepository
{
    public async Task SetProductRatingAsync(Rating item)
    {
        await context.Ratings.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public IQueryable<Rating> Query()
    {
        return context.Ratings.AsQueryable();
    }
}