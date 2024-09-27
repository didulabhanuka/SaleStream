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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtService>();

// Configure JWT and role-based policies
builder.Services.ConfigureJwtAndRoles(builder.Configuration);

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
