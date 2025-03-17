using AutoMapper;
using AirlineBooking.Application.Contracts.Flight;
using AirlineBooking.Application.Contracts;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;

namespace AirlineBooking.Application.Services;

/// <summary>
/// Служба слоя приложения для манипуляции над рейсами
/// </summary>
/// <param name="repository">Доменная служба для рейсов</param>
/// <param name="mapper">Автомаппер</param>
public class FlightCrudService(IFlightRepository repository, IMapper mapper)
    : ICrudService<FlightDto, FlightCreateUpdateDto, int>, IFlightAnalyticsService
{
    /// <summary>
    /// Создание нового рейса
    /// </summary>
    public bool Create(FlightCreateUpdateDto newDto)
    {
        var newFlight = mapper.Map<Flight>(newDto);
        newFlight.Id = repository.GetAll().Max(x => x.Id) + 1;
        var result = repository.Add(newFlight);
        return result;
    }

    /// <summary>
    /// Удаление рейса
    /// </summary>
    public bool Delete(int id) =>
        repository.Delete(id);

    /// <summary>
    /// Получение рейса по ID
    /// </summary>
    public FlightDto? GetById(int id)
    {
        var flight = repository.Get(id);
        return mapper.Map<FlightDto>(flight);
    }

    /// <summary>
    /// Получение всех рейсов
    /// </summary>
    public IList<FlightDto> GetList() =>
        mapper.Map<List<FlightDto>>(repository.GetAll());

    /// <summary>
    /// Обновление данных о рейсе
    /// </summary>
    public bool Update(int key, FlightCreateUpdateDto newDto)
    {
        var oldFlight = repository.Get(key);
        if (oldFlight == null) return false;

        var newFlight = mapper.Map<Flight>(newDto);
        newFlight.Id = key;
        newFlight.Bookings = oldFlight.Bookings; // Сохраняем существующие бронирования
        var result = repository.Update(newFlight);
        return result;
    }

    /// <summary>
    /// Возвращает информацию о всех рейсах в виде списка строк.
    /// </summary>
    public IList<string> GetAllFlightsInfo()
    {
        return repository.GetAll().Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}")
            .ToList();
    }

    /// <summary>
    /// Возвращает список пассажиров для указанного рейса.
    /// </summary>
    public IList<string> GetCustomersByFlight(int flightId)
    {
        var flight = repository.Get(flightId);
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
    public IList<string> GetFlightsByCityAndDate(string departureCity, DateTime date)
    {
        return repository.GetAll()
            .Where(flight => flight.DepartureCity == departureCity && flight.DepartureDate == date)
            .Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}")
            .ToList();
    }

    /// <summary>
    /// Возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    public IList<Tuple<string, int?>> GetTop5FlightsByBookings()
    {
        return repository.GetAll()
            .Where(flight => flight.Bookings != null)
            .OrderByDescending(flight => flight.Bookings.Count)
            .Take(5)
            .Select(flight => new Tuple<string, int?>(flight.FlightNumber, flight.Bookings.Count))
            .ToList();
    }

    /// <summary>
    /// Возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    public IList<string> GetFlightsWithMaxBookings()
    {
        var maxBookings = repository.GetAll()
            .Where(flight => flight.Bookings != null)
            .Max(flight => flight.Bookings.Count);

        return repository.GetAll()
            .Where(flight => flight.Bookings != null && flight.Bookings.Count == maxBookings)
            .Select(flight =>
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Количество бронирований: {flight.Bookings.Count}")
            .ToList();
    }

    /// <summary>
    /// Возвращает статистику бронирований для рейсов, вылетающих из указанного города.
    /// </summary>
    public (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity)
    {
        var flightsFromCity = repository.GetAll()
            .Where(flight => flight.DepartureCity == departureCity && flight.Bookings != null)
            .Select(flight => flight.Bookings.Count)
            .ToList();

        if (flightsFromCity.Count == 0)
            return (0, 0, 0);

        var minBookings = flightsFromCity.Min();
        var maxBookings = flightsFromCity.Max();
        var averageBookings = flightsFromCity.Average();

        return (minBookings, averageBookings, maxBookings);
    }
}