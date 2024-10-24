using DataLayer.Data.Models;

namespace DataLayer.Data.Interfaces;

public interface IRatingRepository
{
    Task SetProductRatingAsync(Rating item);
    IQueryable<Rating> Query();
}