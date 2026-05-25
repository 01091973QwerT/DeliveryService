using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Abstractions;
using DeliveryService.Infrastructure.Data;

namespace DeliveryService.Infrastructure.Repositories;

public class SenderRepository : EfRepository<Sender, Guid>, ISenderRepository
{
    public SenderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Sender?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Phone.Value == phone, cancellationToken);
    }

    public async Task<IReadOnlyList<Delivery>> GetSenderDeliveriesAsync(Guid senderId, CancellationToken cancellationToken = default)
    {
        return await _context.Deliveries
            .Where(d => d.Sender.Id == senderId)
            .ToListAsync(cancellationToken);
    }
}
