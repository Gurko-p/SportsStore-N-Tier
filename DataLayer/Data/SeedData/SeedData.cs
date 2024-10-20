using DataLayer.Data.Contexts;
using DataLayer.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Data.SeedData;

public static class SeedData
{
    public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        if (!await context.Categories.AnyAsync())
        {
            await context.Categories.AddRangeAsync(
                new Category()
                {
                    CategoryName = "Футбол",
                },
                new Category()
                {
                    CategoryName = "Водные виды спорта",
                },
                new Category()
                {
                    CategoryName = "Шахматы",
                });
        }

        await context.SaveChangesAsync();

        // Проверка и добавление товаров
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(
                new Product
                {
                    Name = "Каяк",
                    Description = "Лодка для одного человека",
                    CategoryId = 2,
                    Price = 275
                },
                new Product
                {
                    Name = "Спасательный жилет",
                    Description = "Защитный и модный",
                    CategoryId = 2,
                    Price = 48.95m
                },
                new Product
                {
                    Name = "Футбольный мяч",
                    Description = "Одобренный FIFA размер и вес",
                    CategoryId = 1,
                    Price = 19.50m
                },
                new Product
                {
                    Name = "Угловые флаги",
                    Description = "Придайте вашему полю профессиональный вид",
                    CategoryId = 1,
                    Price = 34.95m
                },
                new Product
                {
                    Name = "Стадион",
                    Description = "Складной стадион на 35,000 мест",
                    CategoryId = 1,
                    Price = 79500
                },
                new Product
                {
                    Name = "Шапка для размышлений",
                    Description = "Увеличьте эффективность мозга на 75%",
                    CategoryId = 3,
                    Price = 16
                },
                new Product
                {
                    Name = "Нестабильный стул",
                    Description = "Секретно дайте своему противнику недостаток",
                    CategoryId = 3,
                    Price = 29.95m
                },
                new Product
                {
                    Name = "Человеческая шахматная доска",
                    Description = "Веселая игра для всей семьи",
                    CategoryId = 3,
                    Price = 75
                },
                new Product
                {
                    Name = "Король с блестками",
                    Description = "Позолоченный, усыпанный бриллиантами король",
                    CategoryId = 3,
                    Price = 1200
                },
                // Добавляем дополнительные товары
                new Product
                {
                    Name = "Футбольные бутсы",
                    Description = "Комфортные бутсы для игры в футбол",
                    CategoryId = 1,
                    Price = 59.99m
                },
                new Product
                {
                    Name = "Водный мяч",
                    Description = "Мяч для водного поло",
                    CategoryId = 2,
                    Price = 25.00m
                },
                new Product
                {
                    Name = "Шахматные фигуры",
                    Description = "Набор фигур для игры в шахматы",
                    CategoryId = 3,
                    Price = 45.00m
                },
                new Product
                {
                    Name = "Футбольные ворота",
                    Description = "Комплект мини-ворот для футбольного поля",
                    CategoryId = 1,
                    Price = 150.00m
                },
                new Product
                {
                    Name = "Плавательный круг",
                    Description = "Круг для плавания для детей",
                    CategoryId = 2,
                    Price = 15.00m
                },
                new Product
                {
                    Name = "Шахматный стол",
                    Description = "Стол для игры в шахматы",
                    CategoryId = 3,
                    Price = 200.00m
                },
                new Product
                {
                    Name = "Футбольный мяч для тренировок",
                    Description = "Мяч для тренировок и игр",
                    CategoryId = 1,
                    Price = 30.00m
                },
                new Product
                {
                    Name = "Плавательная доска",
                    Description = "Доска для обучения плаванию",
                    CategoryId = 2,
                    Price = 20.00m
                },
                new Product
                {
                    Name = "Шахматный набор",
                    Description = "Полный набор для игры в шахматы",
                    CategoryId = 3,
                    Price = 100.00m
                },
                new Product
                {
                    Name = "Футбольный мяч для детей",
                    Description = "Мяч для юных футболистов",
                    CategoryId = 1,
                    Price = 15.00m
                },
                new Product
                {
                    Name = "Спасательный круг",
                    Description = "Круг для спасения на воде",
                    CategoryId = 2,
                    Price = 35.00m
                },
                new Product
                {
                    Name = "Шахматная доска",
                    Description = "Классическая шахматная доска",
                    CategoryId = 3,
                    Price = 50.00m
                }
            );
            await context.SaveChangesAsync();
        }

        // Проверка и добавление рейтинга
        if (!await context.Ratings.AnyAsync())
        {
            await context.Ratings.AddRangeAsync(
                        new Rating()
                        {
                            ProductId = 1,
                            RatingValue = 4,
                            UserId = "admin@example.com",
                            CreatedAt = DateTime.Now
                        },
                        new Rating()
                        {
                            ProductId = 1,
                            RatingValue = 5,
                            UserId = "admin@example.com",
                            CreatedAt = DateTime.Now
                        }
                    );
            await context.SaveChangesAsync();
        }
    }
}