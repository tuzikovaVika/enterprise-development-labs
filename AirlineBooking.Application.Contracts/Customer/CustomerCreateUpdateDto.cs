namespace AirlineBooking.Application.Contracts.Customer;

/// <summary>
/// Dto для создания или изменения клиента
/// </summary>
/// <param name="Passport">Паспортные данные</param>
/// <param name="FullName">ФИО клиента</param>
/// <param name="BirthDate">Дата рождения</param>
public record CustomerCreateUpdateDto(
    string Passport,
    string FullName,
    DateTime BirthDate
);