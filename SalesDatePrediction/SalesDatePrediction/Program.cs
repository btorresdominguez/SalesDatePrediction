using Microsoft.EntityFrameworkCore;
using SalesDatePrediction.Interfaces;
using SalesDatePrediction.Interfaces.RepositoryPattern;
using SalesDatePrediction.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura el DbContext
builder.Services.AddDbContext<SalesDatePredictionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreSampleConnection")));

// Registrar el repositorio para la inyección de dependencias
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()  // Allow all origins
               .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyHeader(); // Allow any header
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
