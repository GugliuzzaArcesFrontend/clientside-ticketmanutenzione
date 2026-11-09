namespace clientside_ticketmanutenzione.Services;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading;

using Microsoft.Extensions.Caching.Memory;

using clientside_ticketmanutenzione.Models;


public sealed class AuthSessionManager(IMemoryCache _cache)
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _refreshLocks = new();
    public string CreateSession(AuthSession session)
    ///<summary>Crea una nuova sessione in memory partendo 
    ///da un oggetto AuthSession</summary>
    {
        var sessionId = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        
        _cache.Set(
            sessionId, 
            session,
            new MemoryCacheEntryOptions 
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            });
        return sessionId;
    }

    public AuthSession? GetSession(string sessionId)
    ///<summary>Lettura sessione in memory</summary>
    {
        return _cache.Get<AuthSession>(sessionId);
    }

    public void RemoveSession(string sessionId)
    ///<summary>Rimozione sessione</summary>
    {
        _cache.Remove(sessionId);
    }

    public SemaphoreSlim GetRefreshLock(string sessionId)
    {
        return _refreshLocks.GetOrAdd(
            sessionId,
            _ => new SemaphoreSlim(1, 1));
    }
}
