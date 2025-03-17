namespace AirlineBooking.Domain.Services;

/// <summary>
///     Интерфейс репозитория для работы с сущностями.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TKey">Тип ключа сущности.</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    ///     Возвращает все сущности из репозитория.
    /// </summary>
    /// <returns>Список всех сущностей.</returns>
    Task<IList<TEntity>> GetAll();

    /// <summary>
    ///     Возвращает сущность по её уникальному ключу.
    /// </summary>
    /// <param name="key">Ключ сущности.</param>
    /// <returns>Сущность или null, если сущность не найдена.</returns>
    Task<TEntity?> Get(TKey key);

    /// <summary>
    ///     Добавляет новую сущность в репозиторий.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    /// <returns>True, если добавление успешно; иначе false.</returns>
    Task<TEntity> Add(TEntity entity);

    /// <summary>
    ///     Обновляет существующую сущность в репозитории.
    /// </summary>
    /// <param name="entity">Обновленная сущность.</param>
    /// <returns>True, если обновление успешно; иначе false.</returns>
    Task<TEntity> Update(TEntity entity);

    /// <summary>
    ///     Удаляет сущность из репозитория по её уникальному ключу.
    /// </summary>
    /// <param name="key">Ключ сущности для удаления.</param>
    /// <returns>True, если удаление успешно; иначе false.</returns>
    Task<bool> Delete(TKey key);
}