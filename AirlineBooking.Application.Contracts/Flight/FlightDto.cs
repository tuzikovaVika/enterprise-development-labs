namespace AirlineBooking.Application.Contracts.Flight;

/// <summary>
/// Dto для просмотра сведений о рейсе
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="FlightNumber">Номер рейса</param>
/// <param name="DepartureCity">Город вылета</param>
/// <param name="ArrivalCity">Город прилета</param>
/// <param name="AircraftType">Тип самолета (из справочника)</param>
/// <param name="DepartureDate">Дата отправления</param>
/// <param name="ArrivalDate">Дата прибытия</param>
/// <param name="BookingCount">Количество бронирований</param>
public record FlightDto(
    int Id,
    string FlightNumber,
    string DepartureCity,
    string ArrivalCity,
    string AircraftType,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    int? BookingCount
);