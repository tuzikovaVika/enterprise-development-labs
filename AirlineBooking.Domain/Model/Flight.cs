namespace AirlineBooking.Domain.Model;

/// <summary>
/// Класс, представляющий рейс.
/// </summary>
public class Flight
{
    /// <summary>
    /// Уникальный идентификатор рейса.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер рейса.
    /// </summary>
    public required string FlightNumber { get; set; }

    /// <summary>
    /// Город вылета.
    /// </summary>
    public string? DepartureCity { get; set; }

    /// <summary>
    /// Город прибытия.
    /// </summary>
    public string? ArrivalCity { get; set; }

    /// <summary>
    /// Тип самолета.
    /// </summary>
    public string? AircraftType { get; set; }

    /// <summary>
    /// Дата и время вылета.
    /// </summary>
    public DateTime? DepartureDate { get; set; }

    /// <summary>
    /// Дата и время прибытия.
    /// </summary>
    public DateTime? ArrivalDate { get; set; }

    /// <summary>
    /// Список бронирований, связанных с рейсом.
    /// </summary>
    public virtual List<Booking>? Bookings { get; set; } = [];

    /// <summary>
    /// Возвращает количество бронирований для рейса.
    /// </summary>
    public int? BookingCount => Bookings?.Count;

    /// <summary>
    /// Вычисляет сумму ID рейсов, связанных с бронированиями текущего рейса.
    /// </summary>
    /// <returns>Сумма ID рейсов или 0, если бронирования отсутствуют.</returns>
    public int? GetFlightCount()
    {
        var sum = 0;
        if (Bookings?.Count > 0)
            foreach (var ba in Bookings)
                if (ba != null && ba.Flight != null)
                    sum += ba.Flight.Id;
        return sum;
    }
}