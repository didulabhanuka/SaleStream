using MongoDB.Driver;
using SaleStream.Repositories;
using SaleStream.Services;
using SaleStream.Configurations;

var builder = WebApplication.CreateBuilder(args);

// MongoDB connection configuration
builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("ConnectionStrings:MongoDB")));

// Register repositories and services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtService>();

// Register repositories and services
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductService>();

// Register repositories and services
builder.Services.AddScoped<NotificationRepository>();
builder.Services.AddScoped<NotificationService>();

// Register repositories and services
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderService>();

// Register services and repositories
builder.Services.AddScoped<VendorRepository>();
builder.Services.AddScoped<VendorService>();

// Configure JWT and role-based policies
builder.Services.ConfigureJwtAndRoles(builder.Configuration);

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Specify your React app's URL
               .AllowAnyMethod() // Allow all HTTP methods
               .AllowAnyHeader() // Allow all headers
               .AllowCredentials(); // Allow credentials if needed
    });
});


var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
