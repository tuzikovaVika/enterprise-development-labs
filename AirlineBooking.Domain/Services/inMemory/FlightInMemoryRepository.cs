using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Data;

namespace AirlineBooking.Domain.Services.InMemory;

/// <summary>
/// Реализация репозитория рейсов в памяти.
/// </summary>
public class FlightInMemoryRepository : IFlightRepository
{
    private List<Flight> _flights;

    /// <summary>
    /// Инициализирует экземпляр класса и загружает данные из DataSeeder.
    /// </summary>
    public FlightInMemoryRepository()
    {
        _flights = DataSeeder.Flights;
    }

    /// <summary>
    /// Добавляет рейс в коллекцию.
    /// </summary>
    /// <param name="entity">Рейс для добавления.</param>
    /// <returns>True, если добавление успешно; иначе false.</returns>
    public bool Add(Flight entity)
    {
        try
        {
            _flights.Add(entity);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Удаляет рейс из коллекции по его ID.
    /// </summary>
    /// <param name="key">ID рейса для удаления.</param>
    /// <returns>True, если удаление успешно; иначе false.</returns>
    public bool Delete(int key)
    {
        try
        {
            var flight = Get(key);
            if (flight != null)
                _flights.Remove(flight);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Обновляет информацию о рейсе в коллекции.
    /// </summary>
    /// <param name="entity">Обновленный рейс.</param>
    /// <returns>True, если обновление успешно; иначе false.</returns>
    public bool Update(Flight entity)
    {
        try
        {
            Delete(entity.Id);
            Add(entity);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Возвращает рейс по его ID.
    /// </summary>
    /// <param name="key">ID рейса.</param>
    /// <returns>Рейс или null, если рейс не найден.</returns>
    public Flight? Get(int key) =>
        _flights.FirstOrDefault(item => item.Id == key);

    /// <summary>
    /// Возвращает все рейсы из коллекции.
    /// </summary>
    /// <returns>Список всех рейсов.</returns>
    public IList<Flight> GetAll() =>
        _flights;

    /// <summary>
    /// Возвращает информацию о всех рейсах в виде списка строк.
    /// </summary>
    /// <returns>Список строк с деталями рейсов.</returns>
    public IList<string> GetAllFlightsInfo()
    {
        return _flights.Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}")
            .ToList();
    }

    /// <summary>
    /// Возвращает список пассажиров для указанного рейса.
    /// </summary>
    /// <param name="flightId">ID рейса.</param>
    /// <returns>Список строк с информацией о пассажирах.</returns>
    public IList<string> GetCustomersByFlight(int flightId)
    {
        var flight = Get(flightId);
        if (flight == null || flight.Bookings == null)
            return new List<string>();

        return flight.Bookings
            .Where(booking => booking.Customer != null)
            .Select(booking => booking.Customer)
            .OrderBy(customer => customer.FullName)
            .Select(customer =>
                $"Пассажир: {customer.FullName}, Паспорт: {customer.Passport}, Дата рождения: {customer.BirthDate}")
            .ToList();
    }

    /// <summary>
    /// Возвращает рейсы, вылетающие из указанного города в указанную дату.
    /// </summary>
    /// <param name="departureCity">Город вылета.</param>
    /// <param name="date">Дата вылета.</param>
    /// <returns>Список строк с информацией о рейсах.</returns>
    public IList<string> GetFlightsByCityAndDate(string departureCity, DateTime date)
    {
        return _flights
            .Where(flight => flight.DepartureCity == departureCity && flight.DepartureDate == date)
            .Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}")
            .ToList();
    }

    /// <summary>
    /// Возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    /// <returns>Список кортежей, содержащих номер рейса и количество бронирований.</returns>
    public IList<Tuple<string, int?>> GetTop5FlightsByBookings()
    {
        return _flights
            .Where(flight => flight.Bookings != null)
            .OrderByDescending(flight => flight.BookingCount)
            .Take(5)
            .Select(flight => new Tuple<string, int?>(flight.FlightNumber, flight.BookingCount))
            .ToList();
    }

    /// <summary>
    /// Возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    /// <returns>Список строк с информацией о рейсах и их бронированиях.</returns>
    public IList<string> GetFlightsWithMaxBookings()
    {
        var maxBookings = _flights
            .Where(flight => flight.Bookings != null)
            .Max(flight => flight.BookingCount);

        return _flights
            .Where(flight => flight.Bookings != null && flight.Bookings.Count == maxBookings)
            .Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Количество бронирований: {flight.BookingCount}")
            .ToList();
    }

    /// <summary>
    /// Возвращает статистику бронирований для рейсов, вылетающих из указанного города.
    /// </summary>
    /// <param name="departureCity">Город вылета.</param>
    /// <returns>Кортеж с минимальным, средним и максимальным количеством бронирований.</returns>
    public (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity)
    {
        var flightsFromCity = _flights
            .Where(flight => flight.DepartureCity == departureCity && flight.Bookings != null)
            .Select(flight => flight.BookingCount)
            .ToList();

        if (flightsFromCity.Count == 0)
            return (0, 0, 0);

        var minBookings = flightsFromCity.Min();
        var maxBookings = flightsFromCity.Max();
        var averageBookings = flightsFromCity.Average();

        return (minBookings, averageBookings, maxBookings);
    }
}