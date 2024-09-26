using System.ComponentModel.DataAnnotations;

namespace SportStore.server.Requests;

public class AuthBaseDto
{
    [Required(ErrorMessage = "Email is required.")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }
}