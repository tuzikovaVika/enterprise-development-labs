namespace AirlineBooking.Application.Contracts.Booking;

/// <summary>
/// Dto для создания или изменения бронирования
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="FlightId">Идентификатор рейса</param>
/// <param name="TicketNumber">Номер билета</param>
public record BookingCreateUpdateDto(
    int CustomerId,
    int FlightId,
    string TicketNumber
);