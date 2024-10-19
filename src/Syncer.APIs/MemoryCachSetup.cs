
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Syncer.APIs.Persistence;
using System.Reflection.Metadata.Ecma335;

namespace Syncer.APIs;

public class MemoryCachSetup(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scoped = _serviceScopeFactory.CreateScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<SyncerDbContext>();

        var emojies = await dbContext.Emojis.ToListAsync();
        if (!emojies.Any()) { return; }

        var cache = scoped.ServiceProvider.GetRequiredService<IMemoryCache>();
        foreach (var emoji in emojies)
            cache.Set(emoji.Code, emoji.SortName);
    }

    public Task StopAsync(CancellationToken cancellationToken)
          => Task.CompletedTask;
}
