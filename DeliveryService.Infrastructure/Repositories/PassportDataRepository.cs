using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Abstractions;
using DeliveryService.Infrastructure.Data;

namespace DeliveryService.Infrastructure.Repositories;

public class PassportDataRepository : EfRepository<PassportData, Guid>, IPassportDataRepository
{
    public PassportDataRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PassportData?> GetBySeriesAndNumberAsync(string series, string number, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Series.Value == series && p.Number.Value == number, cancellationToken);
    }
}
