using IOIT.Identity.Application.Common.Attributes;
using IOIT.Identity.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentAdminService<Guid> _currentAdminService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentAdminService<Guid> currentAdminService
            )
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentAdminService = currentAdminService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var type = typeof(TRequest);
            var method = type.Name;

            _logger.LogInformation($"[MonitorLog]- Start call service method: {request}.{method}");
            var argumentText = "[MonitorLog]- Parameters passing: No Log Parameter.";

            var ignoreLoggingAttribute = type.GetCustomAttributes(typeof(IgnoreLoggingAttribute))
                                  .FirstOrDefault() as IgnoreLoggingAttribute;

            if (ignoreLoggingAttribute == null)
            {
                argumentText = $"[MonitorLog]- Parameters passing: {JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}";
            }
            _logger.LogInformation(argumentText);
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                _logger.LogInformation($"[MonitorLog]- End call service method: {request}.{method}. It takes: {elapsedMilliseconds} milliseconds. User: 1");
            }
            return response;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var type = typeof(TRequest);
            var method = type.Name;

            _logger.LogInformation($"[MonitorLog]- Start call service method: {request}.{method}");
            var argumentText = "[MonitorLog]- Parameters passing: No Log Parameter.";

            var ignoreLoggingAttribute = type.GetCustomAttributes(typeof(IgnoreLoggingAttribute))
                                  .FirstOrDefault() as IgnoreLoggingAttribute;

            if (ignoreLoggingAttribute == null)
            {
                argumentText = $"[MonitorLog]- Parameters passing: {JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}";
            }
            _logger.LogInformation(argumentText);
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                _logger.LogInformation($"[MonitorLog]- End call service method: {request}.{method}. It takes: {elapsedMilliseconds} milliseconds. User: 1");
            }
            return response;
        }
    }
}
