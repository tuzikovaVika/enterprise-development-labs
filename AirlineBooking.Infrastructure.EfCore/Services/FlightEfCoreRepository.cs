using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AirlineBooking.Infrastructure.EfCore.Services;

/// <summary>
///     Реализация репозитория для рейсов в базе данных.
/// </summary>
public class FlightEfCoreRepository(AirlineBookingDbContext context) : IFlightRepository
{
    private readonly DbSet<Flight> _flights = context.Flights;

    /// <summary>
    ///     Добавляет рейс в базу данных.
    /// </summary>
    public async Task<Flight> Add(Flight entity)
    {
        EntityEntry<Flight> result = await _flights.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    ///     Удаляет рейс из базы данных по его ID.
    /// </summary>
    public async Task<bool> Delete(int key)
    {
        Flight? entity = await _flights.FirstOrDefaultAsync(e => e.Id == key);
        if (entity == null)
        {
            return false;
        }

        _flights.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    ///     Возвращает рейс по его ID.
    /// </summary>
    public async Task<Flight?> Get(int key)
    {
        return await _flights.FirstOrDefaultAsync(e => e.Id == key);
    }

    /// <summary>
    ///     Возвращает все рейсы из базы данных.
    /// </summary>
    public async Task<IList<Flight>> GetAll()
    {
        return await _flights.ToListAsync();
    }

    /// <summary>
    ///     Обновляет информацию о рейсе в базе данных.
    /// </summary>
    public async Task<Flight> Update(Flight entity)
    {
        _flights.Update(entity);
        await context.SaveChangesAsync();
        return (await Get(entity.Id))!;
    }

    /// <summary>
    ///     Возвращает информацию о всех рейсах в виде списка строк.
    /// </summary>
    public async Task<IList<string>> GetAllFlightsInfo()
    {
        IList<Flight> flights = await GetAll();
        return flights.Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}")
            .ToList();
    }

    /// <summary>
    ///     Возвращает список пассажиров для указанного рейса.
    /// </summary>
    public async Task<IList<string>> GetCustomersByFlight(int flightId)
    {
        Flight? flight = await Get(flightId);
        if (flight == null || flight.Bookings == null)
        {
            return new List<string>();
        }

        return flight.Bookings
            .Where(booking => booking.Customer != null)
            .Select(booking => booking.Customer)
            .OrderBy(customer => customer!.FullName) // Используем оператор ! для подавления предупреждений
            .Select(customer =>
                $"Пассажир: {customer.FullName}, Паспорт: {customer.Passport}, Дата рождения: {customer.BirthDate}")
            .ToList();
    }

    /// <summary>
    ///     Возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    public async Task<IList<Tuple<string, int?>>> GetTop5FlightsByBookings()
    {
        IList<Flight> flights = await GetAll();
        return flights
            .Where(flight => flight.Bookings != null)
            .OrderByDescending(flight => flight.BookingCount)
            .Take(5)
            .Select(flight => new Tuple<string, int?>(flight.FlightNumber, flight.BookingCount))
            .ToList();
    }

    /// <summary>
    ///     Возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    public async Task<IList<string>> GetFlightsWithMaxBookings()
    {
        IList<Flight> flights = await GetAll();
        var maxBookings = flights
            .Where(flight => flight.Bookings != null)
            .Max(flight => flight.BookingCount);

        return flights
            .Where(flight => flight.Bookings != null && flight.BookingCount == maxBookings)
            .Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Количество бронирований: {flight.BookingCount}")
            .ToList();
    }

    /// <summary>
    ///     Возвращает статистику бронирований для рейсов, вылетающих из указанного города.
    /// </summary>
    public async Task<(int? Min, double? Average, int? Max)> GetBookingStatisticsByCity(string departureCity)
    {
        IList<Flight> flights = await GetAll();
        var flightsFromCity = flights
            .Where(flight => flight.DepartureCity == departureCity && flight.Bookings != null)
            .Select(flight => flight.BookingCount)
            .ToList();

        if (flightsFromCity.Count == 0)
        {
            return (0, 0, 0);
        }

        var minBookings = flightsFromCity.Min();
        var maxBookings = flightsFromCity.Max();
        var averageBookings = flightsFromCity.Average();

        return (minBookings, averageBookings, maxBookings);
    }

    /// <summary>
    ///     Возвращает рейсы, вылетающие из указанного города в указанную дату.
    /// </summary>
    public async Task<IList<string>> GetFlightsByCityAndDate(string departureCity, DateTime departureDate)
    {
        IList<Flight> flights = await GetAll();
        return flights
            .Where(flight => flight.DepartureCity == departureCity && flight.DepartureDate == departureDate.Date)
            .Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}")
            .ToList();
    }
}