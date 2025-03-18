using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;
using AirlineBooking.Domain.Services.InMemory;
using Xunit;

namespace AirlineBooking.Domain.Tests;

public class FlightTests
{
    private readonly FlightService _flightService;

    public FlightTests()
    {
        var repository = new FlightInMemoryRepository();
        _flightService = new FlightService(repository);
    }

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

    [Theory]
    [InlineData(999)]
    public void GetCustomersByFlight_ReturnsEmptyList_WhenNoBookingsExist(int flightId)
    {
        IList<string> result = _flightService.GetCustomersByFlight(flightId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

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

    [Theory]
    [InlineData("Лондон", "15.10.2023 10:00:00")]
    public void GetFlightsByCityAndDate_ReturnsEmptyList_WhenNoFlightsExist(string departureCity, string dateString)
    {
        var date = DateTime.ParseExact(dateString, "dd.MM.yyyy H:mm:ss", null);

        IList<string> result = _flightService.GetFlightsByCityAndDate(departureCity, date);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

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