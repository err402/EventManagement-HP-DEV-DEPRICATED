using EventManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:44386");

// Add CORS configuration before other middleware
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://10.0.0.113:4200"  // Explicitly allow your IP
            )
            .SetIsOriginAllowed(origin => 
                    origin.StartsWith("http://10.0.0.") && origin.EndsWith(":4200") // For any other IPs in your subnet
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// attempt to get this fucking workflow to work just adding this for the commit trigger
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comment out HTTPS redirection since we're using HTTP
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
