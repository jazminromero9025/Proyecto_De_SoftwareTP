using Application.Interfaces;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Obtener cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
