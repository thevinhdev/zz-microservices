using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Common.Interfaces
{
    public interface ICacheService
    {
        T GetCache<T>(string key);
        object GetCache(string key, Type type);
        T GetCache<T>(string key, Func<T> acquire);
        T GetCache<T>(string key, int cacheTime, Func<T> acquire);
        void SaveCache<T>(string key, int cacheTime, Func<T> acquire);
        void SaveCache<T>(string key, int cacheTime, T result);
        void SaveCache<T>(string key, Func<T> acquire);
        void SaveCache<T>(string key, T result);

        Task<T> GetCacheAsync<T>(string key, Func<Task<T>> acquire);
        Task<T> GetCacheAsync<T>(string key, int cacheTime, Func<Task<T>> acquire);
        Task SaveCacheAsync<T>(string key, Func<Task<T>> acquire);
        Task SaveCacheAsync<T>(string key, int cacheTime, Func<Task<T>> acquire);

        void RemoveCache(string key);
        Task RemoveCacheAsync(string key);
    }
}
