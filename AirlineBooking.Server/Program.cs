using AirlineBooking.Application;
using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Booking;
using AirlineBooking.Application.Contracts.Customer;
using AirlineBooking.Application.Contracts.Flight;
using AirlineBooking.Application.Services;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;
using AirlineBooking.Infrastructure.EfCore;
using AirlineBooking.Infrastructure.EfCore.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()));
IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IRepository<Flight, int>, FlightEfCoreRepository>();
builder.Services.AddTransient<IRepository<Customer, int>, CustomerEfCoreRepository>();
builder.Services.AddTransient<IRepository<Booking, int>, BookingEfCoreRepository>();
builder.Services.AddTransient<IFlightRepository, FlightEfCoreRepository>();

builder.Services.AddScoped<ICrudService<FlightDto, FlightCreateUpdateDto, int>, FlightCrudService>();
builder.Services.AddScoped<ICrudService<CustomerDto, CustomerCreateUpdateDto, int>, CustomerCrudService>();
builder.Services.AddScoped<ICrudService<BookingDto, BookingCreateUpdateDto, int>, BookingCrudService>();
builder.Services.AddScoped<IFlightAnalyticsService, FlightCrudService>();

builder.Services.AddDbContextFactory<AirlineBookingDbContext>(options =>
    options.UseLazyLoadingProxies().UseMySql(builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))));

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://localhost:5244")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("default");
app.MapControllers();

app.Run();