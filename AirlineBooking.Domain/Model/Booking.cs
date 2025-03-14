namespace AirlineBooking.Domain.Model;

/// <summary>
/// Класс, представляющий бронирование.
/// </summary>
public class Booking
{
    /// <summary>
    /// Уникальный идентификатор бронирования.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// ID рейса, связанного с бронированием.
    /// </summary>
    public required int FlightId { get; set; }

    /// <summary>
    /// Рейс, связанный с бронированием.
    /// </summary>
    public virtual Flight? Flight { get; set; }

    /// <summary>
    /// ID клиента, связанного с бронированием.
    /// </summary>
    public required int CustomerId { get; set; }

    /// <summary>
    /// Клиент, связанный с бронированием.
    /// </summary>
    public virtual Customer? Customer { get; set; }

    /// <summary>
    /// Номер билета для бронирования.
    /// </summary>
    public required string TicketNumber { get; set; }
}