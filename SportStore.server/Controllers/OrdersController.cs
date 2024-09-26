using DataLayer.Data.Infrastructure;
using DataLayer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportStore.server.Requests;

namespace SportStore.server.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize]
public class OrdersController(DataManager dataManager) : ControllerBase
{

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Orders()
    {
        var orders =
            await dataManager.Orders.Query().AsNoTracking().ToListAsync();
        return Ok(orders);
    }

    [HttpGet]
    [Route("myOrders")]
    public async Task<IActionResult> MyOrders(string userId)
    {
        var orders =
            await dataManager.Carts.Query()
                .Include(x => x.Order)
                .Include(x => x.Product)
                .Where(x => x.Order!.UserId == userId)
                .GroupBy(x => new { x.OrderId, x.Order!.OrderDate, x.Order.Address })
                .AsNoTracking()
                .Select(x => new
                {
                    x.Key.OrderId,
                    x.Key.OrderDate,
                    x.Key.Address,
                    Products = x.Where(a => a.OrderId == x.Key.OrderId)
                        .Select(a => new
                        {
                            a.Quantity,
                            a.Product!.Category!.CategoryName,
                            a.Product.Name,
                            a.Product.Description,
                            a.Product.Price
                        })
                })

                .ToListAsync();
        return Ok(orders);
    }


    [HttpGet]
    [Route("item/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id должен быть больше 0");
        }
        var product = await dataManager.Orders.FirstOrDefaultAsync(id);
        if (product is null)
        {
            return NotFound($"Заказ с id={id} не найден.");
        }
        return Ok(product);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        order.OrderDate ??= DateTime.Now;
        await dataManager.Orders.CreateAsync(order);
        return CreatedAtAction(nameof(Create), new { id = order.Id }, order);
    }

    [HttpPost]
    [Route("createOrderCarts")]
    public async Task<IActionResult> CreateOrderCart([FromBody] OrderDto? order)
    {
        if (order is null || string.IsNullOrEmpty(order.UserId) || string.IsNullOrEmpty(order.Address) || order.Carts is null || !order.Carts.Any())
        {
            return BadRequest("Некоррекный запрос на добавление заказа");
        }
        Order createdOrder = null;
        await using (var transaction = await dataManager.ApplicationDbContext.Database.BeginTransactionAsync())
        {
            try
            {
                createdOrder = await dataManager.Orders.CreateWithReturnCreatedAsync(new Order { Address = order.Address, OrderDate = DateTime.Now, UserId = order.UserId });
                var carts = order.Carts?.Select(cart => new Cart
                {
                    OrderId = createdOrder.Id,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity
                }).ToArray();
                if (carts != null && carts.Any())
                {
                    await dataManager.Carts.CreateRangeAsync(carts);
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { error = "Ошибка на сервере. Попробуйте позже." });
            }
        }
        return CreatedAtAction(nameof(Create), new { id = createdOrder.Id }, createdOrder);
    }


    [HttpPut]
    [Route("update/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }
        await dataManager.Orders.UpdateAsync(order);
        return NoContent();
    }

    [HttpDelete]
    [Route("remove/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await dataManager.Orders.DeleteAsync(id);
        return NoContent();
    }
}