using Fitin.Application.Authentication;

using Fitin.Infrastructure.Auth;
using Fitin.Infrastructure.Persistence;
using Fitin.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Fitin.Infrastructure.Services;
using Fitin.Infrastructure.Settings;
using Fitin.Application.Cart.Interfaces;
using Fitin.Application.Products.Interfaces;
using Fitin.Application.Authentication.Interfaces;
using Fitin.Application.Wishlist.Interfaces;
using Fitin.Application.Common.Mappings;
using AutoMapper;
using Fitin.Application.Products.Services;
using Fitin.Application.Cart.Services;
using Fitin.Application.Wishlist.Services;
using Fitin.API.Middleware;
using Fitin.Application.Orders.Interface;
using Fitin.Application.Orders.Service;
using Fitin.Application.Common.Interfaces;
using Fitin.Application.Users.Interfaces;
using Fitin.Application.Users.Services;
using Fitin.Application.Categories.Interface;
using Fitin.Application.Categories.Services;
using Fitin.Application.Payments.Interfaces;
using Fitin.Application.Payments.Services;



var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IImageService, CloudinaryImageService>();
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddSingleton<AutoMapper.IConfigurationProvider>(_ =>
    new AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.AddProfile<ProductProfile>();
        cfg.AddProfile<WishlistProfile>();
        cfg.AddProfile<CartProfile>();
        cfg.AddProfile<OrderProfile>();
        cfg.AddProfile<UserProfile>();
        cfg.AddProfile<CategoryProfile>();
    }, null));
builder.Services.AddScoped<IMapper>(sp =>
    sp.GetRequiredService<AutoMapper.IConfigurationProvider>().CreateMapper(sp.GetService));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, CloudinaryImageService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["accessToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowFrontend",
    policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

//Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapControllers();

// Seed admin
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await dbContext.Database.MigrateAsync();
    await DbSeeder.SeedAdminAsync(dbContext, configuration, app.Environment.IsDevelopment());
}

app.Run();
