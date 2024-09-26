using DataLayer.Data.Infrastructure;
using DataLayer.Data.SeedData;
using SportStore.server.Helpers;
using SportStore.server.Installers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddScoped<JwtHelper>();
builder.Services.AddDataAccess(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", o => 
        o.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthenticationConfiguration(configuration);
builder.Services.AddAuthorization();

// custom services


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

await IdentitySeedData.EnsurePopulatedAsync(app, configuration);
await SeedData.EnsurePopulatedAsync(app);

app.MapControllers();

app.Run();
