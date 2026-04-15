namespace DeliveryService.Domain.Base;

/// <summary>
/// Базовый класс для всех сущностей домена
/// </summary>
/// <typeparam name="TId">Тип идентификатора (обычно Guid)</typeparam>
public abstract class Entity<TId>(TId id) where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Уникальный идентификатор сущности
    /// </summary>
    public TId Id { get; } = id;

    /// <summary>
    /// Конструктор для EF Core
    /// </summary>
    protected Entity() : this(default!) { }
}
