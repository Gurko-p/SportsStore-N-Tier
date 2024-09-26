using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.server.Helpers;
using SportStore.server.Requests;
using System.Security.Claims;
using DataLayer.Data.Models;

namespace SportStore.server.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(UserManager<ApplicationUser> userManager, JwtHelper jwtHelper, RoleManager<IdentityRole> roleManager) : ControllerBase
{
    [HttpPost("register")]
    [Authorize(Roles= "admin")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        if (!string.IsNullOrEmpty(request.Role))
        {
            if (await roleManager.RoleExistsAsync(request.Role!))
                await userManager.AddToRoleAsync(user, request.Role!);

            else
            {
                return BadRequest($"Роль {request.Role} не найдена!");
            }
        }
        else
        {
            await userManager.AddToRoleAsync(user, "user");
        }
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password)) 
            return Unauthorized();
        var token = jwtHelper.GetJwtTokenAsync(user);
        return Ok(new { Token = token });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        await Task.Delay(0);
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        return Ok(new { userName = userId, roles });
    }
}