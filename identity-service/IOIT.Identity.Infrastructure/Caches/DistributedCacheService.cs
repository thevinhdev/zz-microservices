using IOIT.Identity.Application.Common.Interfaces;
using IOIT.Identity.Application.Common.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Caches
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly int _cacheTime;
        private readonly CacheDataTypes _cacheDataType;
        private const int DefaultCacheTime = 2;
        private const int Invalid = -1;
        private const int DefaultResetCacheTime = 0;

        public DistributedCacheService(IDistributedCache distributedCache, IOptionsSnapshot<CacheOptions> cacheOption)
        {
            this._distributedCache = distributedCache;
            this._cacheTime = cacheOption.Value?.CacheTime ?? DefaultCacheTime;
            this._cacheDataType = cacheOption.Value?.Type ?? CacheDataTypes.Json;
        }
        public T GetCache<T>(string key)
        {
            return _distributedCache.GetCache<T>(key, DefaultResetCacheTime, _cacheDataType);
        }
        public object GetCache(string key, Type type)
        {
            return _distributedCache.GetCache(key, type, DefaultResetCacheTime, _cacheDataType);
        }
        public T GetCache<T>(string key, Func<T> acquire)
        {
            return GetCache(key, _cacheTime, acquire);
        }

        public T GetCache<T>(string key, int cacheTime, Func<T> acquire)
        {
            T result = GetCache<T>(key);
            if (result == null)
            {
                _distributedCache.SetCache(key, acquire(), cacheTime, _cacheDataType);
                result = GetCache<T>(key);
            }
            return result;
        }

        public void SaveCache<T>(string key, int cacheTime, Func<T> acquire)
        {
            T result = acquire();
            _distributedCache.SetCache(key, result, cacheTime, _cacheDataType);
        }
        public void SaveCache<T>(string key, int cacheTime, T result)
        {
            _distributedCache.SetCache(key, result, cacheTime, _cacheDataType);
        }
        public void SaveCache<T>(string key, Func<T> acquire)
        {
            SaveCache<T>(key, _cacheTime, acquire);
        }

        public void SaveCache<T>(string key, T result)
        {
            SaveCache<T>(key, _cacheTime, result);
        }

        public async Task<T> GetCacheAsync<T>(string key, Func<Task<T>> acquire)
        {
            return await GetCacheAsync(key, _cacheTime, acquire);
        }

        public async Task<T> GetCacheAsync<T>(string key, int cacheTime, Func<Task<T>> acquire)
        {
            T result = await _distributedCache.GetCacheAsync<T>(key, DefaultResetCacheTime, _cacheDataType);
            if (result != null)
            {
                return result;
            }
            else
            {
                result = await acquire();
                _distributedCache.SetCacheAsync(key, result, cacheTime, _cacheDataType);
                return result;
            }
        }

        public async Task SaveCacheAsync<T>(string key, Func<Task<T>> acquire)
        {
            await SaveCacheAsync<T>(key, _cacheTime, acquire);
        }

        public async Task SaveCacheAsync<T>(string key, int cacheTime, Func<Task<T>> acquire)
        {
            T result = await acquire();
            _distributedCache.SetCacheAsync(key, result, cacheTime, _cacheDataType);
        }

        public void RemoveCache(string key)
        {
            _distributedCache.Remove(ConvertCacheKey(key));
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _distributedCache.RemoveAsync(ConvertCacheKey(key));
        }

        private string ConvertCacheKey(string key)
        {
            return key = !string.IsNullOrEmpty(key) && _cacheDataType == CacheDataTypes.Json ? string.Format("{0}.{1}", key, _cacheDataType.ToString()) : key;
        }
    }
}
