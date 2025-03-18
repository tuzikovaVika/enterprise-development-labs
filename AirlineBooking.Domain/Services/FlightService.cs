using AirlineBooking.Domain.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;

    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    /// <summary>
    ///     Возвращает информацию о всех рейсах.
    /// </summary>
    public IList<string> GetAllFlightsInfo()
    {
        return _flightRepository.GetAllFlightsInfo();
    }

    /// <summary>
    ///     Возвращает список пассажиров для указанного рейса.
    /// </summary>
    public IList<string> GetCustomersByFlight(int flightId)
    {
        return _flightRepository.GetCustomersByFlight(flightId);
    }

    /// <summary>
    ///     Возвращает рейсы, вылетающие из указанного города в указанную дату.
    /// </summary>
    public IList<string> GetFlightsByCityAndDate(string departureCity, DateTime date)
    {
        return _flightRepository.GetFlightsByCityAndDate(departureCity, date);
    }

    /// <summary>
    ///     Возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    public IList<Tuple<string, int?>> GetTop5FlightsByBookings()
    {
        return _flightRepository.GetTop5FlightsByBookings();
    }

    /// <summary>
    ///     Возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    public IList<string> GetFlightsWithMaxBookings()
    {
        return _flightRepository.GetFlightsWithMaxBookings();
    }

    /// <summary>
    ///     Возвращает статистику бронирований для рейсов, вылетающих из указанного города.
    /// </summary>
    public (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity)
    {
        return _flightRepository.GetBookingStatisticsByCity(departureCity);
    }
}