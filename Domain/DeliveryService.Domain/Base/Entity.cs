namespace DeliveryService.Domain.Base;

/// <summary>
/// Базовый класс для всех сущностей
/// </summary>
/// <typeparam name="TId">Тип идентификатора</typeparam>
public abstract class Entity<TId> where TId : struct, IEquatable<TId>
{
    public TId Id { get; }

    protected Entity(TId id)
    {
        Id = id;
    }

    protected Entity() : this(default!) { }
}
