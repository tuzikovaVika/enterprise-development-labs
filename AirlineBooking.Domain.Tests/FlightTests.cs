using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services.InMemory;

namespace AirlineBooking.Domain.Tests;

/// <summary>
///     Класс с юнит-тестами
/// </summary>
public class FlightTests
{
    private readonly FlightInMemoryRepository _repository;

    public FlightTests()
    {
        _repository = new FlightInMemoryRepository();
    }

    /// <summary>
    ///     Тест метода, возвращающего информацию о всех рейсах
    /// </summary>
    [Fact]
    public async Task GetAllFlightsInfo_ReturnsCorrectFlightDetailsAsync()
    {
        IList<string>? result = await _repository.GetAllFlightsInfo();
        Assert.NotNull(result);
        Assert.Equal(DataSeeder.Flights.Count, result.Count);

        foreach (Flight? flight in DataSeeder.Flights)
        {
            var expectedInfo =
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Параметризованный тест метода, возвращающего список пассажиров по ID рейса
    /// </summary>
    /// <param name="flightId">ID рейса</param>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetCustomersByFlight_ReturnsCorrectCustomerDetailsAsync(int flightId)
    {
        IList<string>? result = await _repository.GetCustomersByFlight(flightId);

        Assert.NotNull(result);

        var customers = DataSeeder.Bookings
            .Where(b => b.FlightId == flightId)
            .Select(b => b.Customer)
            .OrderBy(c => c.FullName)
            .ToList();

        foreach (Customer? customer in customers)
        {
            var expectedInfo =
                $"Пассажир: {customer.FullName}, Паспорт: {customer.Passport}, Дата рождения: {customer.BirthDate}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Параметризованный тест метода, возвращающего пустой список пассажиров для рейса без бронирований
    /// </summary>
    /// <param name="flightId">ID рейса</param>
    [Theory]
    [InlineData(999)]
    public async Task GetCustomersByFlight_ReturnsEmptyList_WhenNoBookingsExistAsync(int flightId)
    {
        IList<string>? result = await _repository.GetCustomersByFlight(flightId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    ///     Параметризованный тест метода, возвращающего рейсы по городу вылета и дате
    /// </summary>
    /// <param name="departureCity">Город вылета</param>
    /// <param name="dateString">Дата вылета в строковом формате</param>
    [Theory]
    [InlineData("Москва", "15.10.2023 10:00:00")]
    [InlineData("Санкт-Петербург", "15.10.2023 14:00:00")]
    [InlineData("Москва", "16.10.2023 8:00:00")]
    public async Task GetFlightsByCityAndDate_ReturnsCorrectFlightsAsync(string departureCity, string dateString)
    {
        var date = DateTime.ParseExact(dateString, "dd.MM.yyyy H:mm:ss", null);

        IList<string>? result = await _repository.GetFlightsByCityAndDate(departureCity, date);

        Assert.NotEmpty(result);

        var expectedFlights = DataSeeder.Flights
            .Where(f => f.DepartureCity == departureCity && f.DepartureDate == date.Date)
            .ToList();

        foreach (Flight? flight in expectedFlights)
        {
            var expectedInfo =
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Дата вылета: {flight.DepartureDate}, Дата прибытия: {flight.ArrivalDate}, Тип самолета: {flight.AircraftType}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Параметризованный тест метода, возвращающего пустой список рейсов для несуществующих данных
    /// </summary>
    /// <param name="departureCity">Город вылета</param>
    /// <param name="dateString">Дата вылета в строковом формате</param>
    [Theory]
    [InlineData("Лондон", "15.10.2023 10:00:00")]
    public async Task GetFlightsByCityAndDate_ReturnsEmptyList_WhenNoFlightsExistAsync(string departureCity,
        string dateString)
    {
        var date = DateTime.ParseExact(dateString, "dd.MM.yyyy H:mm:ss", null);

        IList<string>? result = await _repository.GetFlightsByCityAndDate(departureCity, date);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    ///     Тест метода, возвращающего топ-5 рейсов по количеству бронирований
    /// </summary>
    [Fact]
    public async Task GetTop5FlightsByBookings_ReturnsTop5FlightsAsync()
    {
        IList<Tuple<string, int?>> result = await _repository.GetTop5FlightsByBookings();
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
    ///     Тест метода, возвращающего рейсы с максимальным количеством бронирований
    /// </summary>
    [Fact]
    public async Task GetFlightsWithMaxBookings_ReturnsFlightsWithMaxBookingsAsync()
    {
        IList<string>? result = await _repository.GetFlightsWithMaxBookings();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var maxBookings = DataSeeder.Flights
            .Where(f => f.Bookings != null)
            .Max(f => f.BookingCount);

        var expectedFlights = DataSeeder.Flights
            .Where(f => f.Bookings != null && f.Bookings.Count == maxBookings)
            .ToList();

        foreach (Flight? flight in expectedFlights)
        {
            var expectedInfo =
                $"Рейс: {flight.FlightNumber}, Откуда: {flight.DepartureCity}, Куда: {flight.ArrivalCity}, " +
                $"Количество бронирований: {flight.BookingCount}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    ///     Параметризованный тест метода, возвращающего статистику бронирований по городу вылета
    /// </summary>
    /// <param name="departureCity">Город вылета</param>
    [Theory]
    [InlineData("Москва")]
    [InlineData("Санкт-Петербург")]
    public async Task GetBookingStatisticsByCity_ReturnsCorrectStatisticsAsync(string departureCity)
    {
        (int? Min, double? Average, int? Max) result = await _repository.GetBookingStatisticsByCity(departureCity);

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