using IOIT.Identity.Application.Common.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Caches
{
    public static class DistributedCacheExtension
    {
        private static string CacheKeyRule(string key, CacheDataTypes dataTypes)
        {
            return $"{key}.{dataTypes.ToString()}";
        }

        public static T GetCache<T>(this IDistributedCache cacheManager, string key, int resetCacheTime, CacheDataTypes dataTypes)
        {
            key = CacheKeyRule(key, dataTypes);

            switch (dataTypes)
            {
                case CacheDataTypes.ByteArray:
                    var data = cacheManager.Get(key);
                    if (data != null && data.Length > 0)
                    {
                        if (resetCacheTime > 0)
                            cacheManager.Set(key, data, new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(resetCacheTime)
                            });

                        return DeserializeBinaryData<T>(data);
                    }
                    break;

                case CacheDataTypes.Json:
                    var jsonData = cacheManager.GetString(key);
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        if (resetCacheTime > 0)
                            cacheManager.SetString(key, jsonData, new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(resetCacheTime)
                            });

                        return DeserializeJsonData<T>(jsonData);
                    }
                    break;
            }

            return default(T);

        }

        public static object GetCache(this IDistributedCache cacheManager, string key, Type type, int resetCacheTime, CacheDataTypes dataType)
        {
            key = CacheKeyRule(key, dataType);
            switch (dataType)
            {
                case CacheDataTypes.ByteArray:
                    var data = cacheManager.Get(key);
                    if (data != null && data.Length > 0)
                    {
                        if (resetCacheTime > 0)
                            cacheManager.Set(key, data, new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(resetCacheTime)
                            });
                        return DeserializeBinaryData(data, type);
                    }
                    break;
                case CacheDataTypes.Json:
                    var jsonData = cacheManager.GetString(key);
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        if (resetCacheTime > 0)
                            cacheManager.SetString(key, jsonData, new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(resetCacheTime)
                            });
                        return JsonConvert.DeserializeObject(jsonData, type);
                    }
                    break;
            }

            return null;
        }

        public static async Task<T> GetCacheAsync<T>(this IDistributedCache cacheManager, string key, int resetCacheTime, CacheDataTypes dataType)
        {
            key = CacheKeyRule(key, dataType);
            switch (dataType)
            {
                case CacheDataTypes.ByteArray:
                    var data = await cacheManager.GetAsync(key);
                    if (data != null && data.Length > 0)
                    {
                        if (resetCacheTime > 0)
                        {
                            await cacheManager.SetAsync(key, data, new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(resetCacheTime)
                            });
                        }
                        return DeserializeBinaryData<T>(data);
                    }

                    break;

                case CacheDataTypes.Json:
                    var jsonData = await cacheManager.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        if (resetCacheTime > 0)
                        {
                            await cacheManager.SetStringAsync(key, jsonData.ToString(), new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(resetCacheTime)
                            });
                        }
                        return DeserializeJsonData<T>(jsonData);
                    }

                    break;
            }

            return default(T);
        }

        public static void SetCache(this IDistributedCache cacheManager, string key, object data, int cacheTime, CacheDataTypes dataType)
        {
            key = CacheKeyRule(key, dataType);
            switch (dataType)
            {
                case CacheDataTypes.ByteArray:
                    cacheManager.Set(key, SerializeBinaryData(data), new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime)
                    });

                    break;

                case CacheDataTypes.Json:
                    cacheManager.SetString(key, SerializeJsonData(data), new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime)
                    });

                    break;
            }
        }

        public static async void SetCacheAsync(this IDistributedCache cacheManager, string key, object data, int cacheTime, CacheDataTypes dataType)
        {
            key = CacheKeyRule(key, dataType);
            switch (dataType)
            {
                case CacheDataTypes.ByteArray:
                    await cacheManager.SetAsync(key, SerializeBinaryData(data), new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime)
                    });

                    break;

                case CacheDataTypes.Json:
                    await cacheManager.SetStringAsync(key, SerializeJsonData(data), new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime)
                    });

                    break;
            }
        }
        private static string SerializeJsonData(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        private static T DeserializeJsonData<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        private static byte[] SerializeBinaryData(object data)
        {
            var input = SerializeJsonData(data);
            return Encoding.ASCII.GetBytes(input);
        }

        private static T DeserializeBinaryData<T>(byte[] data)
        {
            var output = Encoding.ASCII.GetString(data);
            return DeserializeJsonData<T>(output);
        }
        private static object DeserializeBinaryData(byte[] data, Type type)
        {
            var output = Encoding.ASCII.GetString(data);
            return JsonConvert.DeserializeObject(output, type);
        }
    }
}
