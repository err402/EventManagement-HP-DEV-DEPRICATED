var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Reverse Proxy
builder.Services.AddHealthChecks();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comment out HTTPS redirection for now
//app.UseHttpsRedirection();
app.MapHealthChecks("/health");
// Map reverse proxy routes
app.MapReverseProxy();

app.Run();