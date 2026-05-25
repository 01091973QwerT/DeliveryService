using Microsoft.EntityFrameworkCore;

namespace DeliveryService.WebHost.Helpers;

/// <summary>
/// Расширение для применения миграций при старте хоста.
/// </summary>
public static class MigrationManager
{
    public static IHost MigrateDatabase<T>(this IHost host)
        where T : DbContext
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();
        context.Database.Migrate();

        return host;
    }
}
