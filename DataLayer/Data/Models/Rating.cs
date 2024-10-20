namespace DataLayer.Data.Models;

public class Rating
{
    public int Id { get; set; }
    public double RatingValue { get; set; }
    public string? UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}