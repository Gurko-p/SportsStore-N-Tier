using DataLayer.Data.Infrastructure;
using DataLayer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportStore.server.Controllers;

[Route("api/carts")]
[ApiController]
[Authorize]
public class CartsController(DataManager dataManager) : ControllerBase
{

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Carts()
    {
        var carts = 
            await dataManager.Carts.Query()
                .Include(x => x.Product)
                .Include(x => x.Order)
                .ToListAsync();
        return Ok(carts);
    }

    [HttpGet]
    [Route("item/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id должен быть больше 0");
        }
        var cart = await dataManager.Carts.FirstOrDefaultAsync(id);
        if (cart is null)
        {
            return NotFound($"Корзина с id={id} не найден.");
        }
        return Ok(cart);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] Cart cart)
    {
        await dataManager.Carts.CreateAsync(cart);
        return CreatedAtAction(nameof(Create), new { id = cart.Id }, cart);
    }

    [HttpPut]
    [Route("update/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Cart order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }
        await dataManager.Carts.UpdateAsync(order);
        return NoContent();
    }

    [HttpDelete]
    [Route("remove/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await dataManager.Carts.DeleteAsync(id);
        return NoContent();
    }
}