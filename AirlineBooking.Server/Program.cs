using AirlineBooking.Application;
using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Booking;
using AirlineBooking.Application.Contracts.Customer;
using AirlineBooking.Application.Contracts.Flight;
using AirlineBooking.Application.Services;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;
using AirlineBooking.Domain.Services.InMemory;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var mapperConfig = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()));
IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Регистрация репозиториев
builder.Services.AddSingleton<IRepository<Flight, int>, FlightInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Customer, int>, CustomerInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Booking, int>, BookingInMemoryRepository>();

// Регистрация CRUD-служб
builder.Services.AddScoped<ICrudService<FlightDto, FlightCreateUpdateDto, int>, FlightCrudService>();
builder.Services.AddScoped<ICrudService<CustomerDto, CustomerCreateUpdateDto, int>, CustomerCrudService>();
builder.Services.AddScoped<ICrudService<BookingDto, BookingCreateUpdateDto, int>, BookingCrudService>();

// Регистрация аналитической службы
builder.Services.AddScoped<IFlightAnalyticsService, FlightCrudService>();
builder.Services.AddSingleton<IFlightRepository, FlightInMemoryRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirlineBooking API V1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();