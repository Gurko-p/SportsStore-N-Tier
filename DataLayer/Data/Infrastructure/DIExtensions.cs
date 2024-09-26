using DataLayer.Data.Contexts;
using DataLayer.Data.Interfaces;
using DataLayer.Data.Models;
using DataLayer.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Data.Infrastructure
{
    public static class DIExtensions
    {
        private static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Cart>, CartRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<DataManager>();
            return services;
        }

        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);

            services.AddIdentity();

            services.AddDataServices();

            return services;
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
                    opt => opt.MigrationsAssembly(typeof(IdentityApplicationDbContext).Assembly.FullName))
            );

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SportStoreConnectionString"),
                    opt => opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            );
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
