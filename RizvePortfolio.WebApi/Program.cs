using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RizvePortfolio.Application;
using RizvePortfolio.Infrastructure;
using RizvePortfolio.Infrastructure.Persistence;
using RizvePortfolio.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Application & Infrastructure DI
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

// Add authentication/authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure uploads directory exists
var uploadsPath = builder.Configuration["UploadsPath"] ?? "uploads";
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// Seed database in Development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Ensure database is created
    dbContext.Database.EnsureCreated();
    
    // Seed admin user
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Serve static files from uploads folder
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

