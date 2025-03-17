namespace AirlineBooking.Application.Contracts.Customer;

/// <summary>
/// Dto для просмотра сведений о клиенте
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Passport">Паспортные данные</param>
/// <param name="FullName">ФИО клиента</param>
/// <param name="BirthDate">Дата рождения</param>
/// <param name="BookingCount">Количество бронирований</param>
public record CustomerDto(
    int Id,
    string Passport,
    string FullName,
    DateTime BirthDate,
    int? BookingCount = null
);