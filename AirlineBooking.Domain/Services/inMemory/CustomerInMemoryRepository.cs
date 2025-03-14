using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;

namespace AirlineBooking.Domain.Services.InMemory;

/// <summary>
/// Реализация репозитория клиентов в памяти.
/// </summary>
public class CustomerInMemoryRepository : IRepository<Customer, int>
{
    private List<Customer> _customers;

    /// <summary>
    /// Инициализирует экземпляр класса и загружает данные из DataSeeder.
    /// </summary>
    public CustomerInMemoryRepository()
    {
        _customers = DataSeeder.Customers;
    }

    /// <summary>
    /// Добавляет клиента в коллекцию.
    /// </summary>
    /// <param name="entity">Клиент для добавления.</param>
    /// <returns>True, если добавление успешно; иначе false.</returns>
    public bool Add(Customer entity)
    {
        try
        {
            _customers.Add(entity);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Удаляет клиента из коллекции по его ID.
    /// </summary>
    /// <param name="key">ID клиента для удаления.</param>
    /// <returns>True, если удаление успешно; иначе false.</returns>
    public bool Delete(int key)
    {
        try
        {
            var customer = Get(key);
            if (customer != null)
                _customers.Remove(customer);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Возвращает клиента по его ID.
    /// </summary>
    /// <param name="key">ID клиента.</param>
    /// <returns>Клиент или null, если клиент не найден.</returns>
    public Customer? Get(int key) =>
        _customers.FirstOrDefault(item => item.Id == key);

    /// <summary>
    /// Возвращает всех клиентов из коллекции.
    /// </summary>
    /// <returns>Список всех клиентов.</returns>
    public IList<Customer> GetAll() =>
        _customers;

    /// <summary>
    /// Обновляет информацию о клиенте в коллекции.
    /// </summary>
    /// <param name="entity">Обновленный клиент.</param>
    /// <returns>True, если обновление успешно; иначе false.</returns>
    public bool Update(Customer entity)
    {
        try
        {
            Delete(entity.Id);
            Add(entity);
        }
        catch
        {
            return false;
        }

        return true;
    }
}