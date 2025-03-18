using AirlineBooking.Domain.Model;

namespace AirlineBooking.Domain.Services;

/// <summary>
///     Интерфейс репозитория для работы с рейсами.
/// </summary>
public interface IFlightRepository : IRepository<Flight, int>
{
    /// <summary>
    ///     Возвращает информацию о всех рейсах в виде списка строк.
    /// </summary>
    /// <returns>Список строк с деталями рейсов.</returns>
    IList<string> GetAllFlightsInfo();

    /// <summary>
    ///     Возвращает список пассажиров для указанного рейса.
    /// </summary>
    /// <param name="flightId">ID рейса.</param>
    /// <returns>Список строк с информацией о пассажирах.</returns>
    IList<string> GetCustomersByFlight(int flightId);

    /// <summary>
    ///     Возвращает рейсы, вылетающие из указанного города в указанную дату.
    /// </summary>
    /// <param name="departureCity">Город вылета.</param>
    /// <param name="date">Дата вылета.</param>
    /// <returns>Список строк с информацией о рейсах.</returns>
    IList<string> GetFlightsByCityAndDate(string departureCity, DateTime date);

    /// <summary>
    ///     Возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    /// <returns>Список кортежей, содержащих номер рейса и количество бронирований.</returns>
    IList<Tuple<string, int?>> GetTop5FlightsByBookings();

    /// <summary>
    ///     Возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    /// <returns>Список строк с информацией о рейсах и их бронированиях.</returns>
    IList<string> GetFlightsWithMaxBookings();

    /// <summary>
    ///     Возвращает статистику бронирований для рейсов, вылетающих из указанного города.
    /// </summary>
    /// <param name="departureCity">Город вылета.</param>
    /// <returns>Кортеж с минимальным, средним и максимальным количеством бронирований.</returns>
    (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity);
}