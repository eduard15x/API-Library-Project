global using AutoMapper;
using ITPLibrary.Api.Core.Services.BookService;
using ITPLibrary.Api.Core.Services.EmailService;
using ITPLibrary.Api.Core.Services.TokenService;
using ITPLibrary.Api.Core.Services.UserService;
using ITPLibrary.Api.Core.Services.ShoppingCartService;
using ITPLibrary.Api.Core.Services.OrderService;
using ITPLibrary.Api.Core.Services.FavoriteProductService;
using ITPLibrary.Api.Data.EF.Data;
using ITPLibrary.Api.Data.Shared.IRepository;
using ITPLibrary.Api.Data.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Data;
using System.Text;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var DataBaseProvider = config["DatabaseProvider"];
Console.WriteLine("Now we are using the " + DataBaseProvider + " repository to interact with database.");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = """Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);
// extra Services
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDbConnection>(x => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFavoriteProductService, FavoriteProductService>();

if (DataBaseProvider!.ToLower() == "dapper")
{
    builder.Services.AddScoped<IBookRepository, ITPLibrary.Api.Data.SQL.Repositories.BookRepository>();
    builder.Services.AddScoped<IUserRepository, ITPLibrary.Api.Data.SQL.Repositories.UserRepository>();
    builder.Services.AddScoped<IShoppingCartRepository, ITPLibrary.Api.Data.SQL.Repositories.ShoppingCartRepository>();
    builder.Services.AddScoped<IOrderRepository, ITPLibrary.Api.Data.SQL.Repositories.OrderRepository>();
    builder.Services.AddScoped<IFavoriteProductRepository, ITPLibrary.Api.Data.SQL.Repositories.FavoriteProductRepository>();
}
else if (DataBaseProvider.ToLower() == "entityframework")
{
    builder.Services.AddScoped<IBookRepository, ITPLibrary.Api.Data.EF.Repositories.BookRepository>();
    builder.Services.AddScoped<IUserRepository, ITPLibrary.Api.Data.EF.Repositories.UserRepository>();
    builder.Services.AddScoped<IShoppingCartRepository, ITPLibrary.Api.Data.EF.Repositories.ShoppingCartRepository>();
    builder.Services.AddScoped<IOrderRepository, ITPLibrary.Api.Data.EF.Repositories.OrderRepository>();
    builder.Services.AddScoped<IFavoriteProductRepository, ITPLibrary.Api.Data.EF.Repositories.FavoriteProductRepository>();
}
else
{
    throw new InvalidOperationException("Invalid DatabaseProvider configuration.");
}

// correspond to the authentication methods, we added in ProductController the [Authorize] property for our middleware
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:TokenKey").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


// insert secret key values from appsettings in StripeSettings.cs properties
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
