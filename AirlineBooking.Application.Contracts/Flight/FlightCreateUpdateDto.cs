namespace AirlineBooking.Application.Contracts.Flight;

/// <summary>
///     Dto для создания или изменения информации о рейсе
/// </summary>
/// <param name="FlightNumber">Номер рейса</param>
/// <param name="DepartureCity">Город вылета</param>
/// <param name="ArrivalCity">Город прилета</param>
/// <param name="AircraftType">Тип самолета</param>
/// <param name="DepartureDate">Дата отправления</param>
/// <param name="ArrivalDate">Дата прибытия</param>
public record FlightCreateUpdateDto(
    string FlightNumber, // Номер рейса
    string DepartureCity, // Город вылета
    string ArrivalCity, // Город прилета
    string AircraftType, // Тип самолета (из справочника)
    DateTime DepartureDate, // Дата отправления
    DateTime ArrivalDate // Дата прибытия
);