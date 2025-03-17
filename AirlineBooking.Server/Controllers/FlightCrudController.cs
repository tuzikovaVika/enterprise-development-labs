using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Flight;

namespace AirlineBooking.Server.Controllers;

/// <summary>
///     Контроллер для CRUD-операций над рейсами
/// </summary>
/// <param name="crudService">CRUD-служба</param>
public class FlightController(ICrudService<FlightDto, FlightCreateUpdateDto, int> crudService)
    : CrudControllerBase<FlightDto, FlightCreateUpdateDto, int>(crudService);