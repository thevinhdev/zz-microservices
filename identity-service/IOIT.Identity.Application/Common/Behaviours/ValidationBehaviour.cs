using FluentValidation.Results;
using IOIT.Identity.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ValidationException = IOIT.Identity.Application.Common.Exceptions.ValidationException;

namespace IOIT.Identity.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ICacheService _cacheService;
        //private readonly IAsyncRepository<TranslationResource> _asyncRepository;
        private readonly ICurrentAdminService<Guid> _currentAdminService;
        public ValidationBehaviour(ICacheService cacheService, 
            ICurrentAdminService<Guid> currentUserService, 
            //IAsyncRepository<TranslationResource> asyncRepository, 
            IEnumerable<IValidator<TRequest>> validators)
        {
            _cacheService = cacheService;
            //_asyncRepository = asyncRepository;
            _validators = validators;
            _currentAdminService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            List<ValidationFailure> validationFailures;
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                validationFailures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (validationFailures.Count != 0)
                {
                    //await ToLanguages();
                    throw new ValidationException(validationFailures);
                }
            }
            return await next();

            //async Task ToLanguages()
            //{
            //    var translations = await _cacheService.GetCacheAsync<IDictionary<string, TranslationCachedModel>>(CacheKeys.TRANSLATIONRESOURCES, async () =>
            //    {
            //        var trans = await _asyncRepository.ListAllAsync();
            //        return trans.ToDictionary(v => $"{v.Key}.{v.LanguageIsoCode}", v => new TranslationCachedModel(v.Key, v.Value), StringComparer.OrdinalIgnoreCase);
            //    });

            //    var languageCode = request.GetType().GetProperty("LanguageIsoCode")?.GetValue(request, null) ?? _currentUserService.LanguageIsoCode;

            //    foreach (var item in validationFailures)
            //    {
            //        var tran = translations.GeTranslation($"{item.ErrorCode}.{languageCode}", $"{item.ErrorCode}.en");
            //        if (!string.IsNullOrWhiteSpace(tran))
            //            item.ErrorMessage = tran;
            //    }
            //}
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
