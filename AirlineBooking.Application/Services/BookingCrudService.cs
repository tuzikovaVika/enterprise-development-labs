using AutoMapper;
using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Booking;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;

namespace AirlineBooking.Application.Services;

/// <summary>
/// Служба слоя приложения для манипуляции над бронированиями
/// </summary>
/// <param name="repository">Доменная служба для бронирований</param>
/// <param name="customerRepository">Доменная служба для клиентов</param>
/// <param name="flightRepository">Доменная служба для рейсов</param>
/// <param name="mapper">Автомаппер</param>
public class BookingCrudService(
    IRepository<Booking, int> repository,
    IRepository<Customer, int> customerRepository,
    IRepository<Flight, int> flightRepository,
    IMapper mapper)
    : ICrudService<BookingDto, BookingCreateUpdateDto, int>
{
    /// <summary>
    /// Создание нового бронирования
    /// </summary>
    public bool Create(BookingCreateUpdateDto newDto)
    {
        try
        {
            var newBooking = mapper.Map<Booking>(newDto);
            newBooking.Id = repository.GetAll().Max(x => x.Id) + 1;

            var customer = customerRepository.Get(newBooking.CustomerId);
            var flight = flightRepository.Get(newBooking.FlightId);

            if (customer == null || flight == null)
                return false;

            newBooking.Customer = customer;
            newBooking.Flight = flight;

            customer.Bookings.Add(newBooking);
            flight.Bookings.Add(newBooking);

            customerRepository.Update(customer);
            flightRepository.Update(flight);

            return repository.Add(newBooking);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Удаление бронирования
    /// </summary>
    public bool Delete(int id)
    {
        try
        {
            var booking = repository.Get(id);

            if (booking == null)
                return false;

            var customer = customerRepository.Get(booking.CustomerId);
            var flight = flightRepository.Get(booking.FlightId);

            customer?.Bookings?.Remove(booking);
            flight?.Bookings?.Remove(booking);

            customerRepository.Update(customer);
            flightRepository.Update(flight);

            return repository.Delete(id);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Получение бронирования по ID
    /// </summary>
    public BookingDto? GetById(int id)
    {
        var booking = repository.Get(id);
        return mapper.Map<BookingDto>(booking);
    }

    /// <summary>
    /// Получение всех бронирований
    /// </summary>
    public IList<BookingDto> GetList() =>
        mapper.Map<List<BookingDto>>(repository.GetAll());

    /// <summary>
    /// Обновление данных о бронировании
    /// </summary>
    public bool Update(int key, BookingCreateUpdateDto newDto)
    {
        try
        {
            var oldBooking = repository.Get(key);
            if (oldBooking == null)
                return false;

            var newBooking = mapper.Map<Booking>(newDto);
            newBooking.Id = key;

            // Удаляем старое бронирование из связей
            var oldCustomer = customerRepository.Get(oldBooking.CustomerId);
            var oldFlight = flightRepository.Get(oldBooking.FlightId);
            oldCustomer?.Bookings?.Remove(oldBooking);
            oldFlight?.Bookings?.Remove(oldBooking);

            customerRepository.Update(oldCustomer);
            flightRepository.Update(oldFlight);

            // Добавляем новое бронирование в связи
            var newCustomer = customerRepository.Get(newBooking.CustomerId);
            var newFlight = flightRepository.Get(newBooking.FlightId);
            newCustomer?.Bookings?.Add(newBooking);
            newFlight?.Bookings?.Add(newBooking);

            customerRepository.Update(newCustomer);
            flightRepository.Update(newFlight);

            return repository.Update(newBooking);
        }
        catch
        {
            return false;
        }
    }
}