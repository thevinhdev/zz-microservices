using IOIT.Identity.Application.Common.Attributes;
using IOIT.Identity.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Common.Behaviours
{
    public class CacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<TRequest> _logger;

        public CacheBehaviour(ICacheService cacheService, ILogger<TRequest> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var type = typeof(TRequest);
            var cacheAttribute = type.GetCustomAttributes(typeof(AppCacheAttribute))
                .FirstOrDefault() as AppCacheAttribute;

            if (cacheAttribute == null)
            {
                return await next();
            }

            var cacheKey = string.IsNullOrEmpty(cacheAttribute.FixKey) ? BuildKeyBasedOnMethod() : cacheAttribute.FixKey;
            var cacheResponse = _cacheService.GetCache<TResponse>(cacheKey);

            if (cacheResponse != null)
            {
                _logger.LogDebug($"Response retrieved {typeof(TRequest).FullName} from cache. CacheKey: {cacheKey}");

                return cacheResponse;
            }

            var response = await next();
            _logger.LogDebug($"Caching response for {typeof(TRequest).FullName} with cache key: {cacheKey}");

            _cacheService.SaveCache(cacheKey, response);

            return response;

            string BuildKeyBasedOnMethod()
            {
                var method = type.Name;
                var args = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var key = $"{method}_{type}_{args}";

                return ComputeHash(key);

                string ComputeHash(string plainText)
                {
                    var md5 = MD5.Create();

                    var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                    return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                }
            }
        }
    }
}
