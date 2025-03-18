using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;
using AirlineBooking.Domain.Services.InMemory;
using Xunit;

namespace AirlineBooking.Domain.Tests;

/// <summary>
///     Тесты для проверки функциональности, связанной с рейсами (Flights).
/// </summary>
public class FlightTests
{
    private readonly FlightService _flightService;

    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="FlightTests"/>.
    ///     Создает экземпляр сервиса рейсов с использованием репозитория в памяти.
    /// </summary>
    public FlightTests()
    {
        var repository = new FlightInMemoryRepository();
        _flightService = new FlightService(repository);
    }

    /// <summary>
    ///     Проверяет, что метод GetAllFlightsInfo возвращает корректную информацию о всех рейсах.
    /// </summary>
    [Fact]
    public void GetAllFlightsInfo_ReturnsCorrectFlightDetails()
    {
        IList<string> result = _flightService.GetAllFlightsInfo();
        Assert.NotNull(result);
        Assert.Equal(DataSeeder.Flights.Count, result.Count);

        foreach (Flight flight in DataSeeder.Flights)
        {
            var expectedInfo =
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Проверяет, что метод GetCustomersByFlight возвращает корректную информацию о пассажирах для заданного рейса.
    /// </summary>
    /// <param name="flightId">Идентификатор рейса.</param>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetCustomersByFlight_ReturnsCorrectCustomerDetails(int flightId)
    {
        IList<string> result = _flightService.GetCustomersByFlight(flightId);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var customers = DataSeeder.Bookings
            .Where(b => b.FlightId == flightId)
            .Select(b => b.Customer)
            .OrderBy(c => c.FullName)
            .ToList();

        foreach (Customer customer in customers)
        {
            var expectedInfo =
                $"Пассажир: {customer.FullName}, Паспорт: {customer.Passport}, Дата рождения: {customer.BirthDate}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Проверяет, что метод GetCustomersByFlight возвращает пустой список, если бронирования для рейса отсутствуют.
    /// </summary>
    /// <param name="flightId">Идентификатор рейса.</param>
    [Theory]
    [InlineData(999)]
    public void GetCustomersByFlight_ReturnsEmptyList_WhenNoBookingsExist(int flightId)
    {
        IList<string> result = _flightService.GetCustomersByFlight(flightId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    ///     Проверяет, что метод GetFlightsByCityAndDate возвращает корректные рейсы для указанного города и даты.
    /// </summary>
    /// <param name="departureCity">Город отправления.</param>
    /// <param name="dateString">Дата и время отправления в строковом формате.</param>
    [Theory]
    [InlineData("Москва", "15.10.2023 10:00:00")]
    [InlineData("Санкт-Петербург", "15.10.2023 14:00:00")]
    [InlineData("Москва", "16.10.2023 8:00:00")]
    public void GetFlightsByCityAndDate_ReturnsCorrectFlights(string departureCity, string dateString)
    {
        var date = DateTime.ParseExact(dateString, "dd.MM.yyyy H:mm:ss", null);

        IList<string> result = _flightService.GetFlightsByCityAndDate(departureCity, date);

        Assert.NotEmpty(result);

        var expectedFlights = DataSeeder.Flights
            .Where(f => f.DepartureCity == departureCity && f.DepartureDate == date.Date)
            .ToList();

        foreach (Flight flight in expectedFlights)
        {
            var expectedInfo =
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Проверяет, что метод GetFlightsByCityAndDate возвращает пустой список, если рейсы для указанного города и даты отсутствуют.
    /// </summary>
    /// <param name="departureCity">Город отправления.</param>
    /// <param name="dateString">Дата и время отправления в строковом формате.</param>
    [Theory]
    [InlineData("Лондон", "15.10.2023 10:00:00")]
    public void GetFlightsByCityAndDate_ReturnsEmptyList_WhenNoFlightsExist(string departureCity, string dateString)
    {
        var date = DateTime.ParseExact(dateString, "dd.MM.yyyy H:mm:ss", null);

        IList<string> result = _flightService.GetFlightsByCityAndDate(departureCity, date);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    ///     Проверяет, что метод GetTop5FlightsByBookings возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    [Fact]
    public void GetTop5FlightsByBookings_ReturnsTop5Flights()
    {
        IList<Tuple<string, int?>> result = _flightService.GetTop5FlightsByBookings();
        Assert.NotNull(result);
        Assert.True(result.Count <= 5);

        var topFlights = DataSeeder.Flights
            .Where(f => f.Bookings != null)
            .OrderByDescending(f => f.BookingCount)
            .Take(5)
            .ToList();

        foreach (Flight flight in topFlights)
        {
            var expectedTuple = new Tuple<string, int?>(flight.FlightNumber, flight.BookingCount);
            Assert.Contains(expectedTuple, result);
        }
    }

    /// <summary>
    ///     Проверяет, что метод GetFlightsWithMaxBookings возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    [Fact]
    public void GetFlightsWithMaxBookings_ReturnsFlightsWithMaxBookings()
    {
        IList<string> result = _flightService.GetFlightsWithMaxBookings();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var maxBookings = DataSeeder.Flights
            .Where(f => f.Bookings != null)
            .Max(f => f.BookingCount);

        var expectedFlights = DataSeeder.Flights
            .Where(f => f.Bookings != null && f.Bookings.Count == maxBookings)
            .ToList();

        foreach (Flight flight in expectedFlights)
        {
            var expectedInfo =
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Количество бронирований: {flight.BookingCount}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Проверяет, что метод GetBookingStatisticsByCity возвращает корректные статистические данные по бронированиям для указанного города.
    /// </summary>
    /// <param name="departureCity">Город отправления.</param>
    [Theory]
    [InlineData("Москва")]
    [InlineData("Санкт-Петербург")]
    public void GetBookingStatisticsByCity_ReturnsCorrectStatistics(string departureCity)
    {
        (int? Min, double? Average, int? Max) result = _flightService.GetBookingStatisticsByCity(departureCity);

        var bookingsCount = DataSeeder.Flights
            .Where(f => f.DepartureCity == departureCity && f.Bookings != null)
            .Select(f => f.BookingCount)
            .ToList();

        if (bookingsCount.Any())
        {
            Assert.Equal(bookingsCount.Min(), result.Min);
            Assert.Equal(bookingsCount.Max(), result.Max);
            Assert.Equal(bookingsCount.Average(), result.Average);
        }
        else
        {
            Assert.Equal((0, 0, 0), result);
        }
    }
}