namespace AirlineBooking.Domain.Services;

/// <summary>
///     Интерфейс для сервиса, предоставляющего функциональность работы с рейсами.
///     Определяет методы для получения информации о рейсах, пассажирах, статистике бронирований и других данных.
/// </summary>
public interface IFlightService
{
    /// <summary>
    ///     Возвращает информацию о всех рейсах.
    /// </summary>
    /// <returns>Список строк, содержащих информацию о каждом рейсе.</returns>
    IList<string> GetAllFlightsInfo();

    /// <summary>
    ///     Возвращает список пассажиров для указанного рейса.
    /// </summary>
    /// <param name="flightId">Идентификатор рейса.</param>
    /// <returns>Список строк, содержащих информацию о пассажирах рейса.</returns>
    IList<string> GetCustomersByFlight(int flightId);

    /// <summary>
    ///     Возвращает рейсы, вылетающие из указанного города в указанную дату.
    /// </summary>
    /// <param name="departureCity">Город отправления.</param>
    /// <param name="date">Дата вылета.</param>
    /// <returns>Список строк, содержащих информацию о рейсах.</returns>
    IList<string> GetFlightsByCityAndDate(string departureCity, DateTime date);

    /// <summary>
    ///     Возвращает топ-5 рейсов с наибольшим количеством бронирований.
    /// </summary>
    /// <returns>Список кортежей, содержащих номер рейса и количество бронирований.</returns>
    IList<Tuple<string, int?>> GetTop5FlightsByBookings();

    /// <summary>
    ///     Возвращает рейсы с максимальным количеством бронирований.
    /// </summary>
    /// <returns>Список строк, содержащих информацию о рейсах с максимальным количеством бронирований.</returns>
    IList<string> GetFlightsWithMaxBookings();

    /// <summary>
    ///     Возвращает статистику бронирований для рейсов, вылетающих из указанного города.
    /// </summary>
    /// <param name="departureCity">Город отправления.</param>
    /// <returns>Кортеж, содержащий минимальное, среднее и максимальное количество бронирований.</returns>
    (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity);
}