using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;

namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Недопустимый статус доставки для выполнения операции.
/// </summary>
public sealed class InvalidDeliveryStatusException(Delivery delivery, DeliveryStatus currentStatus, string actionDescription)
    : InvalidOperationException(
        $"Невозможно выполнить «{actionDescription}» для доставки id = {delivery.Id}: " +
        $"текущий статус «{currentStatus}» не допускает данную операцию.")
{
    public Delivery Delivery => delivery;
    public DeliveryStatus CurrentStatus => currentStatus;
    public string ActionDescription => actionDescription;
}
