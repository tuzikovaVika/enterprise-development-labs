using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;

namespace AirlineBooking.Domain.Services.InMemory;

/// <summary>
///     Реализация репозитория клиентов в памяти.
/// </summary>
public class CustomerInMemoryRepository : IRepository<Customer, int>
{
    private readonly List<Customer> _customers;

    /// <summary>
    ///     Инициализирует экземпляр класса и загружает данные из DataSeeder.
    /// </summary>
    public CustomerInMemoryRepository()
    {
        _customers = DataSeeder.Customers;
    }

    /// <summary>
    ///     Добавляет клиента в коллекцию.
    /// </summary>
    /// <param name="entity">Клиент для добавления.</param>
    /// <returns>True, если добавление успешно; иначе false.</returns>
    public Task<Customer> Add(Customer entity)
    {
        try
        {
            _customers.Add(entity);
        }
        catch
        {
            return null!;
        }

        return Task.FromResult(entity);
    }

    /// <summary>
    ///     Удаляет клиента из коллекции по его ID.
    /// </summary>
    /// <param name="key">ID клиента для удаления.</param>
    /// <returns>True, если удаление успешно; иначе false.</returns>
    public async Task<bool> Delete(int key)
    {
        try
        {
            Customer? customer = await Get(key);
            if (customer != null)
            {
                _customers.Remove(customer);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Возвращает клиента по его ID.
    /// </summary>
    /// <param name="key">ID клиента.</param>
    /// <returns>Клиент или null, если клиент не найден.</returns>
    public Task<Customer?> Get(int key)
    {
        return Task.FromResult(_customers.FirstOrDefault(item => item.Id == key));
    }

    /// <summary>
    ///     Возвращает всех клиентов из коллекции.
    /// </summary>
    /// <returns>Список всех клиентов.</returns>
    public Task<IList<Customer>> GetAll()
    {
        return Task.FromResult((IList<Customer>)_customers);
    }

    /// <summary>
    ///     Обновляет информацию о клиенте в коллекции.
    /// </summary>
    /// <param name="entity">Обновленный клиент.</param>
    /// <returns>True, если обновление успешно; иначе false.</returns>
    public async Task<Customer> Update(Customer entity)
    {
        try
        {
            await Delete(entity.Id);
            await Add(entity);
        }
        catch
        {
            return null!;
        }

        return entity;
    }
}