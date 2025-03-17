using AirlineBooking.Application.Contracts.Booking;
using AirlineBooking.Application.Contracts.Customer;
using AirlineBooking.Application.Contracts.Flight;
using AirlineBooking.Domain.Model;
using AutoMapper;

namespace AirlineBooking.Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Flight, FlightDto>();
        CreateMap<FlightCreateUpdateDto, Flight>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerCreateUpdateDto, Customer>();

        CreateMap<Booking, BookingDto>();
        CreateMap<BookingCreateUpdateDto, Booking>();
    }
}