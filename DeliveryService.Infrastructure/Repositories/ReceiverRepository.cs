using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Abstractions;
using DeliveryService.Infrastructure.Data;

namespace DeliveryService.Infrastructure.Repositories;

public class ReceiverRepository : EfRepository<Receiver, Guid>, IReceiverRepository
{
    public ReceiverRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Receiver?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Phone.Value == phone, cancellationToken);
    }

    public async Task<IReadOnlyList<Delivery>> GetReceiverDeliveriesAsync(Guid receiverId, CancellationToken cancellationToken = default)
    {
        return await _context.Deliveries
            .Where(d => d.Receiver.Id == receiverId)
            .ToListAsync(cancellationToken);
    }
}
