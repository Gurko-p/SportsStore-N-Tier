using DataLayer.Data.Infrastructure;
using DataLayer.Data.SeedData;
using SportStore.server.Hubs;
using SportStore.server.Installers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddDataAccess(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", o =>
            o.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddAuthenticationConfiguration(configuration);
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ProductRatingHub>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await IdentitySeedData.EnsurePopulatedAsync(app, configuration);
await SeedData.EnsurePopulatedAsync(app);

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ProductRatingHub>("/ratingHub");

app.Run();
