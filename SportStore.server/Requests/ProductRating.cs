namespace SportStore.server.Requests;

public struct ProductRating
{
    public int ProductId { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }
}