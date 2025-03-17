using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Booking;

namespace AirlineBooking.Server.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над бронированиями
/// </summary>
/// <param name="crudService">CRUD-служба</param>
public class BookingController(ICrudService<BookingDto, BookingCreateUpdateDto, int> crudService)
    : CrudControllerBase<BookingDto, BookingCreateUpdateDto, int>(crudService);