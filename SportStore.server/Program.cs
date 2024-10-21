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
            o.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
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

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ProductRatingHub>("/ratingHub");
    endpoints.MapControllers();
});

app.Run();
