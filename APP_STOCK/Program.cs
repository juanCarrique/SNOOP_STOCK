using DataAccess;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Services;
using Services.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => // esto remplaza a builder.Services.AddEndpointsApiExplorer(); para agregar un boton de authorize
{
    //titulo
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stockapp", Version = "v1" });

    //boton authorize (swagger)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
             },
             new string[] {}
        }
     });
});

// Agrego servicios
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ComposicionService>();
builder.Services.AddScoped<ItemFacturaService>();
builder.Services.AddScoped<FacturaService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedoresService>();
builder.Services.AddScoped<ReposicionService>();
// Agrego DAOs
builder.Services.AddScoped<CategoriaDAO>();
builder.Services.AddScoped<ClienteDAO>();
builder.Services.AddScoped<ComposicionDAO>();
builder.Services.AddScoped<FacturaDAO>();
builder.Services.AddScoped<ItemFacturaDAO>();
builder.Services.AddScoped<ProductoDAO>();
builder.Services.AddScoped<ProveedorDAO>();
builder.Services.AddScoped<ReposicionDAO>();
builder.Services.AddScoped<UsuarioDAO>();
builder.Services.AddDbContext<StockappContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

/*
{
  "mail": "carlos@mail.com",
  "password": "password1"
}
*/