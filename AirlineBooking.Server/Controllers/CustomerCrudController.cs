using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Customer;

namespace AirlineBooking.Server.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над клиентами
/// </summary>
/// <param name="crudService">CRUD-служба</param>
public class CustomerController(ICrudService<CustomerDto, CustomerCreateUpdateDto, int> crudService)
    : CrudControllerBase<CustomerDto, CustomerCreateUpdateDto, int>(crudService);