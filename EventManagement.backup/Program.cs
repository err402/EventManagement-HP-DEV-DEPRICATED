using EventManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// We'll use nginx to handle the port forwarding, so we'll use localhost here
builder.WebHost.UseUrls("http://localhost:44386");

// Updated CORS configuration for deployment
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true) // During testing, allow all origins
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add CORS middleware before other middleware
app.UseCors("AllowAngular");

// Enable Swagger in production temporarily for testing
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// Add a health check endpoint
app.MapGet("/health", () => "Healthy");

app.Run();