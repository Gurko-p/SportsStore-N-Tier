namespace SportStore.server.Requests;

public class RegisterDto : AuthBaseDto
{
    public string? Role { get; set; } = "user";
}