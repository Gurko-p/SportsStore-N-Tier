namespace SportStore.server.Requests
{
    public class OrderDto
    {
        public string? UserId { get; set; }
        public string? Address { get; set; }
        public CartDto[]? Carts { get; set; }
    }
}
