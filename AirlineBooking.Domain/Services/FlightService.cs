namespace AirlineBooking.Domain.Services;

/// <summary>
///     Сервис для работы с рейсами (Flights).
///     Этот класс предоставляет методы для получения информации о рейсах, пассажирах, статистике бронирований и других данных.
/// </summary>
public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;

    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="FlightService"/>.
    /// </summary>
    /// <param name="flightRepository">Репозиторий для работы с данными о рейсах.</param>
    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    /// <summary>
    ///     Возвращает информацию о всех рейсах.
    /// </summary>
    /// <returns>Список строк, содержащих информацию о каждом рейсе.</returns>
    public IList<string> GetAllFlightsInfo()
    {
        return _flightRepository.GetAllFlightsInfo();
    }

    /// <summary>
    ///     Возвращает список пассажиров для указанного рейса.
    /// </summary>
    /// <param name="flightId">Идентификатор рейса.</param>
    /// <returns>Список строк, содержащих информацию о пассажирах рейса.</returns>
    public IList<string> GetCustomersByFlight(int flightId)
    {
        return _flightRepository.GetCustomersByFlight(flightId);
    }

    /// <summary>
    ///     Возвращает рейсы, вылетающие из указанного города в указанную дату.
    /// </summary>
    /// <param name="departureCity">Город отправления.</param>
    /// <param name="date">Дата вылета.</param>
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
    /// <param name="departureCity">Город отправления.</param>
    public (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity)
    {
        return _flightRepository.GetBookingStatisticsByCity(departureCity);
    }
}