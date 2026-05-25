using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Repositories.Abstractions;
using DeliveryService.Infrastructure.Data;

namespace DeliveryService.Infrastructure.Repositories;

public class DeliveryRepository : EfRepository<Delivery, Guid>, IDeliveryRepository
{
    public DeliveryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Delivery>> GetBySenderIdAsync(Guid senderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Sender)
            .Include(d => d.Receiver)
            .Where(d => d.Sender.Id == senderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Delivery>> GetByReceiverIdAsync(Guid receiverId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Sender)
            .Include(d => d.Receiver)
            .Where(d => d.Receiver.Id == receiverId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Delivery>> GetByStatusAsync(DeliveryStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Delivery>> GetByPaymentStatusAsync(PaymentStatus paymentStatus, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.PaymentStatus == paymentStatus)
            .ToListAsync(cancellationToken);
    }
}