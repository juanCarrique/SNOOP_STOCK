using DAO_STOCK.Context;
using DAO_STOCK;
using Microsoft.EntityFrameworkCore;
using SERVICES_STOCK;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Lbuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Agrego servicios
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProductoDAO>();
// Agrego DAOs
builder.Services.AddScoped<CategoriaDAO>();
builder.Services.AddDbContext<StockappContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
