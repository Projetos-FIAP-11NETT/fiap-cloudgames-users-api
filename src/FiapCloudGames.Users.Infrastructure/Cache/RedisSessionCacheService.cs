using System.Text.Json;
using FiapCloudGames.Users.Application.Sessions;
using Microsoft.Extensions.Caching.Distributed;

namespace FiapCloudGames.Users.Infrastructure.Cache;

public sealed class RedisSessionCacheService(IDistributedCache cache) : ISessionCacheService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task StoreAsync(SessionCacheEntry session, TimeSpan expiration, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(session, JsonSerializerOptions);

        await cache.SetStringAsync(
            GetSessionKey(session.SessionId),
            payload,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            },
            cancellationToken);
    }

    public async Task<SessionCacheEntry?> GetAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var payload = await cache.GetStringAsync(GetSessionKey(sessionId), cancellationToken);

        return string.IsNullOrWhiteSpace(payload)
            ? null
            : JsonSerializer.Deserialize<SessionCacheEntry>(payload, JsonSerializerOptions);
    }

    public Task RemoveAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(GetSessionKey(sessionId), cancellationToken);
    }

    private static string GetSessionKey(Guid sessionId) => $"sessions:{sessionId}";
}
