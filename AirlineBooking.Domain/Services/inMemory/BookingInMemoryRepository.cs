using AirlineBooking.Domain.Data;
using AirlineBooking.Domain.Model;

namespace AirlineBooking.Domain.Services.InMemory;

/// <summary>
///     Реализация репозитория бронирований в памяти.
/// </summary>
public class BookingInMemoryRepository : IRepository<Booking, int>
{
    private readonly List<Booking> _bookings;

    /// <summary>
    ///     Инициализирует экземпляр класса и загружает данные из DataSeeder.
    /// </summary>
    public BookingInMemoryRepository()
    {
        _bookings = DataSeeder.Bookings;
    }

    /// <summary>
    ///     Добавляет бронирование в коллекцию.
    /// </summary>
    /// <param name="entity">Бронирование для добавления.</param>
    /// <returns>True, если добавление успешно; иначе false.</returns>
    public bool Add(Booking entity)
    {
        try
        {
            _bookings.Add(entity);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Удаляет бронирование из коллекции по его ID.
    /// </summary>
    /// <param name="key">ID бронирования для удаления.</param>
    /// <returns>True, если удаление успешно; иначе false.</returns>
    public bool Delete(int key)
    {
        try
        {
            Booking? booking = Get(key);
            if (booking != null)
            {
                _bookings.Remove(booking);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Возвращает бронирование по его ID.
    /// </summary>
    /// <param name="key">ID бронирования.</param>
    /// <returns>Бронирование или null, если бронирование не найдено.</returns>
    public Booking? Get(int key)
    {
        return _bookings.FirstOrDefault(item => item.Id == key);
    }

    /// <summary>
    ///     Возвращает все бронирования из коллекции.
    /// </summary>
    /// <returns>Список всех бронирований.</returns>
    public IList<Booking> GetAll()
    {
        return _bookings;
    }

    /// <summary>
    ///     Обновляет информацию о бронировании в коллекции.
    /// </summary>
    /// <param name="entity">Обновленное бронирование.</param>
    /// <returns>True, если обновление успешно; иначе false.</returns>
    public bool Update(Booking entity)
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