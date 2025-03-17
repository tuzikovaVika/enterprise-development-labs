namespace AirlineBooking.Application.Contracts;

/// <summary>
/// Интерфейс для примитивной CRUD-службы в системе бронирования авиабилетов
/// </summary>
/// <typeparam name="TDto">Dto для просмотра сущности</typeparam>
/// <typeparam name="TCreateUpdateDto">Dto для создания или обновления сущности</typeparam>
/// <typeparam name="TKey">Тип первичного ключа сущности</typeparam>
public interface ICrudService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Создание новой сущности (например, нового рейса, клиента или бронирования)
    /// </summary>
    /// <param name="newDto">Dto для создания сущности</param>
    /// <returns>Индикатор успешности операции</returns>
    public bool Create(TCreateUpdateDto newDto);

    /// <summary>
    /// Обновление существующей сущности (например, данных о рейсе или клиенте)
    /// </summary>
    /// <param name="key">Идентификатор сущности</param>
    /// <param name="newDto">Dto для обновления сущности</param>
    /// <returns>Индикатор успешности операции</returns>
    public bool Update(TKey key, TCreateUpdateDto newDto);

    /// <summary>
    /// Удаление сущности (например, удаление рейса, клиента или бронирования)
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Индикатор успешности операции</returns>
    public bool Delete(TKey id);

    /// <summary>
    /// Получение коллекции всех сущностей (например, всех рейсов, клиентов или бронирований)
    /// </summary>
    /// <returns>Коллекция сущностей</returns>
    public IList<TDto> GetList();

    /// <summary>
    /// Получение сущности по идентификатору (например, рейса или клиента по ID)
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность</returns>
    public TDto? GetById(TKey id);
}