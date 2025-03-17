using AutoMapper;
using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Customer;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;

namespace AirlineBooking.Application.Services;

/// <summary>
/// Служба слоя приложения для манипуляции над клиентами
/// </summary>
/// <param name="repository">Доменная служба для клиентов</param>
/// <param name="mapper">Автомаппер</param>
public class CustomerCrudService(IRepository<Customer, int> repository, IMapper mapper) 
    : ICrudService<CustomerDto, CustomerCreateUpdateDto, int>
{
    /// <summary>
    /// Создание нового клиента
    /// </summary>
    public bool Create(CustomerCreateUpdateDto newDto)
    {
        var newCustomer = mapper.Map<Customer>(newDto);
        newCustomer.Id = repository.GetAll().Max(x => x.Id) + 1;
        var result = repository.Add(newCustomer);
        return result;
    }

    /// <summary>
    /// Удаление клиента
    /// </summary>
    public bool Delete(int id) =>
        repository.Delete(id);

    /// <summary>
    /// Получение клиента по ID
    /// </summary>
    public CustomerDto? GetById(int id)
    {
        var customer = repository.Get(id);
        return mapper.Map<CustomerDto>(customer);
    }

    /// <summary>
    /// Получение всех клиентов
    /// </summary>
    public IList<CustomerDto> GetList() =>
        mapper.Map<List<CustomerDto>>(repository.GetAll());

    /// <summary>
    /// Обновление данных о клиенте
    /// </summary>
    public bool Update(int key, CustomerCreateUpdateDto newDto)
    {
        var oldCustomer = repository.Get(key);
        if (oldCustomer == null) return false;

        var newCustomer = mapper.Map<Customer>(newDto);
        newCustomer.Id = key;
        newCustomer.Bookings = oldCustomer.Bookings; // Сохраняем существующие бронирования
        var result = repository.Update(newCustomer);
        return result;
    }
}