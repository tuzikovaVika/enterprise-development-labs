using AirlineBooking.Application.Contracts;
using AirlineBooking.Application.Contracts.Customer;
using AirlineBooking.Domain.Model;
using AirlineBooking.Domain.Services;
using AutoMapper;

namespace AirlineBooking.Application.Services;

/// <summary>
///     Служба слоя приложения для манипуляции над клиентами
/// </summary>
/// <param name="repository">Доменная служба для клиентов</param>
/// <param name="mapper">Автомаппер</param>
public class CustomerCrudService(IRepository<Customer, int> repository, IMapper mapper)
    : ICrudService<CustomerDto, CustomerCreateUpdateDto, int>
{
    /// <summary>
    ///     Создание нового клиента
    /// </summary>
    public async Task<CustomerDto> Create(CustomerCreateUpdateDto newDto)
    {
        Customer? newCustomer = mapper.Map<Customer>(newDto);
        newCustomer.Id = (await repository.GetAll()).Max(x => x.Id) + 1;
        Customer? res = await repository.Add(newCustomer);
        return mapper.Map<CustomerDto>(res);
    }

    /// <summary>
    ///     Удаление клиента
    /// </summary>
    public async Task<bool> Delete(int id)
    {
        return await repository.Delete(id);
    }

    /// <summary>
    ///     Получение клиента по ID
    /// </summary>
    public async Task<CustomerDto?> GetById(int id)
    {
        Customer? customer = await repository.Get(id);
        return customer != null ? mapper.Map<CustomerDto>(customer) : null;
    }

    /// <summary>
    ///     Получение всех клиентов
    /// </summary>
    public async Task<IList<CustomerDto>> GetList()
    {
        return mapper.Map<List<CustomerDto>>(await repository.GetAll());
    }

    /// <summary>
    ///     Обновление данных о клиенте
    /// </summary>
    public async Task<CustomerDto> Update(int key, CustomerCreateUpdateDto newDto)
    {
        Customer? newCustomer = mapper.Map<Customer>(newDto);
        await repository.Update(newCustomer);
        return mapper.Map<CustomerDto>(newCustomer);
    }
}