using DataLayer.Data.Infrastructure;
using DataLayer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportStore.server.Controllers;

[Route("api/products")]
[ApiController]
[Authorize]
public class ProductsController(DataManager dataManager) : ControllerBase
{

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Products()
    {
        var products =
                await dataManager.Products.Query()
                    .Include(x => x.Category)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Price,
                        x.Description,
                        x.Stock,
                        x.CategoryId,
                        x.Category,
                        Rating = x.Ratings != null && x.Ratings.Count > 0
                            ? 
                            new
                            {
                                TotalRates = x.Ratings.Count,
                                OverallRating = x.Ratings.Average(a => a.RatingValue)
                            }
                            : null
                    })
                    .ToListAsync();
        return Ok(products);
    }

    [HttpGet]
    [Route("list/chunk")]
    public async Task<IActionResult> ProductsChunk(int page = 1, int pageSize = 10, int? filterByCategoryId = 0)
    {
        var products =
            await dataManager.Products.Query()
                .Where(x => filterByCategoryId == 0 || x.CategoryId == filterByCategoryId)
                .AsNoTracking()
                .Include(x => x.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        var totalProducts = await dataManager.Products.Query()
            .CountAsync(x => filterByCategoryId == 0
                             || x.CategoryId == filterByCategoryId);

        return Ok(new
        {
            TotalCount = totalProducts,
            Products = products
        });
    }


    [HttpGet]
    [Route("item/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id должен быть больше 0");
        }
        var product = await dataManager.Products.FirstOrDefaultAsync(id);
        if (product is null)
        {
            return NotFound($"Продукт с id={id} не найден.");
        }
        return Ok(product);
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        await dataManager.Products.CreateAsync(product);
        return CreatedAtAction(nameof(Create), new { id = product.Id }, product);
    }

    [HttpPut]
    [Route("update/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }
        await dataManager.Products.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete]
    [Route("remove/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await dataManager.Products.DeleteAsync(id);
        return NoContent();
    }
}