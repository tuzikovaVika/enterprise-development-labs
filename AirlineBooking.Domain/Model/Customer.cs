namespace AirlineBooking.Domain.Model;

/// <summary>
///     Класс, представляющий клиента.
/// </summary>
public class Customer
{
    /// <summary>
    ///     Уникальный идентификатор клиента.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///     Номер паспорта клиента.
    /// </summary>
    public string? Passport { get; set; }

    /// <summary>
    ///     Полное имя клиента.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    ///     Дата рождения клиента.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    ///     Список бронирований, связанных с клиентом.
    /// </summary>
    public virtual List<Booking>? Bookings { get; set; }

    /// <summary>
    ///     Возвращает количество бронирований клиента.
    /// </summary>
    public int? BookingCount => Bookings?.Count;
}