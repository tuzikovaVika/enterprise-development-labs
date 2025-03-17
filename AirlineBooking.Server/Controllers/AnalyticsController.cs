using AirlineBooking.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBooking.Server.Controllers;

/// <summary>
///     Контроллер для выполнения аналитических запросов по рейсам
/// </summary>
/// <param name="service">Служба для выполнения аналитических запросов</param>
[Route("api/[controller]")]
[ApiController]
public class FlightAnalyticsController(IFlightAnalyticsService service) : ControllerBase
{
    /// <summary>
    ///     Получение топ-5 рейсов по количеству бронирований
    /// </summary>
    /// <returns>Список кортежей (номер рейса, количество бронирований)</returns>
    [HttpGet("Top5Flights")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<Tuple<string, int?>>>> GetTop5Flights()
    {
        return Ok(await service.GetTop5FlightsByBookings());
    }

    /// <summary>
    ///     Получение рейсов с максимальным количеством бронирований
    /// </summary>
    /// <returns>Список строк с информацией о рейсах и их бронированиях</returns>
    [HttpGet("MaxBookingsFlights")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<string>>> GetFlightsWithMaxBookings()
    {
        return Ok(await service.GetFlightsWithMaxBookings());
    }

    /// <summary>
    ///     Получение статистики бронирований для рейсов, вылетающих из указанного города
    /// </summary>
    /// <param name="departureCity">Город вылета</param>
    /// <returns>Кортеж с минимальным, средним и максимальным количеством бронирований</returns>
    [HttpGet("BookingStatistics/{departureCity}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<(int? Min, double? Average, int? Max)>> GetBookingStatistics(string departureCity)
    {
        (int? Min, double? Average, int? Max) statistics = await service.GetBookingStatisticsByCity(departureCity);
        return Ok(statistics);
    }
}