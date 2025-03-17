namespace AirlineBooking.Application.Contracts.Booking;

/// <summary>
///     Dto для просмотра сведений о бронировании
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="FlightId">Идентификатор рейса</param>
public record BookingDto(
    int Id, // Идентификатор бронирования
    int CustomerId, // Идентификатор клиента
    int FlightId // Идентификатор рейса
);