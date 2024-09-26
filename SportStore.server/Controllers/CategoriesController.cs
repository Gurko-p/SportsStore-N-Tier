using DataLayer.Data.Infrastructure;
using DataLayer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportStore.server.Controllers;

[Route("api/categories")]
[ApiController]
[Authorize]
public class CategoriesController(DataManager dataManager) : ControllerBase
{

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Categories()
    {
        var categories = 
            await dataManager.Categories.Query().AsNoTracking()
                .ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("item/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id должен быть больше 0");
        }
        var cart = await dataManager.Categories.FirstOrDefaultAsync(id);
        if (cart is null)
        {
            return NotFound($"Корзина с id={id} не найден.");
        }
        return Ok(cart);
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        await dataManager.Categories.CreateAsync(category);
        return CreatedAtAction(nameof(Create), new { id = category.Id }, category);
    }

    [HttpPut]
    [Route("update/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }
        await dataManager.Categories.UpdateAsync(category);
        return NoContent();
    }

    [HttpDelete]
    [Route("remove/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await dataManager.Categories.DeleteAsync(id);
        return NoContent();
    }
}