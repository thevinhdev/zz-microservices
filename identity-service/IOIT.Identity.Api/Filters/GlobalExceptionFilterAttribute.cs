using IOIT.Identity.Api.Models;
using IOIT.Identity.Api.Options;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using Constants = IOIT.Identity.Application.Common.Constants;

namespace IOIT.Identity.Api.Filters
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IOptionsSnapshot<AppOptions> _settings;
        private readonly ICacheService _cacheService;
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly IMediator _mediator;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public GlobalExceptionFilterAttribute(ICacheService cacheService
            , IOptionsSnapshot<AppOptions> settings
            , IMediator mediator)
        {
            _settings = settings;
            _mediator = mediator;
            _cacheService = cacheService;
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(BadRequestException), HandleBadRequestException},
                { typeof(RequiredTokenExeption), HandleRequiredTokenException},
                { typeof(CommonException), HandleCommmonException},
                { typeof(UnpermissionException), HandlePermissionException},
                { typeof(UnknowException), HandleErrorUnknownException}
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            //if (context.ModelState.IsValid)
            //{
            //    HandleInvalidModelStateException(context);
            //    return;
            //}

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new DefaultResponse().Error(context.Exception.Message, Constants.StatusCodeResApi.Error500))
            {
                StatusCode = Constants.StatusCodeResApi.Error500
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", context.Exception.Message);
            logData.Properties.Add("ErrorCode", ApiConstants.ErrorCode.ERROR_SERVER);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            context.Result = new OkObjectResult(new DefaultResponse().Error(exception.Message, Constants.StatusCodeResApi.Error422))
            {
                StatusCode = Constants.StatusCodeResApi.Error422
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", context.Exception.Message);
            logData.Properties.Add("ErrorCode", ApiConstants.ErrorCode.ERROR_VALIDATION);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result = new OkObjectResult(new DefaultResponse().Error(exception != null ? exception.Message : context.Exception.Message, Constants.StatusCodeResApi.Error422))
            {
                StatusCode = Constants.StatusCodeResApi.Error404
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", exception.Message);
            logData.Properties.Add("ErrorCode", exception.ErrorCode);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;

            context.Result = new OkObjectResult(new DefaultResponse().Error(context.Exception.Message, Constants.StatusCodeResApi.Error404));

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", exception.Message);
            logData.Properties.Add("ErrorCode", exception.ErrorCode);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            context.Result = new OkObjectResult(new DefaultResponse().Error(context.Exception.Message, Constants.StatusCodeResApi.Error422))
            {
                StatusCode = Constants.StatusCodeResApi.Error500
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", context.Exception.Message);
            logData.Properties.Add("ErrorCode", ApiConstants.ErrorCode.ERROR_SERVER);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleRequiredTokenException(ExceptionContext context)
        {
            var exception = context.Exception as RequiredTokenExeption;

            context.Result = new OkObjectResult(new DefaultResponse().Error(exception != null ? exception.Message : Resources.EXCEPTION_UNKNOWN, Constants.StatusCodeResApi.Error401))
            {
                StatusCode = Constants.StatusCodeResApi.Error401
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", exception.Message);
            logData.Properties.Add("ErrorCode", exception.ErrorCode);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleCommmonException(ExceptionContext context)
        {
            var exception = context.Exception as CommonException;

            context.Result = new OkObjectResult(new DefaultResponse().Error(
                   exception != null ? exception.Message : Resources.EXCEPTION_UNKNOWN,
                   exception.StatusCode
               ))
            {
                StatusCode = Constants.StatusCodeResApi.Success200
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", exception.Message);
            logData.Properties.Add("ErrorCode", exception.ErrorCode);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandlePermissionException(ExceptionContext context)
        {
            var exception = context.Exception as UnpermissionException;

            context.Result = new OkObjectResult(new DefaultResponse().Error(exception != null ? exception.Message : Resources.EXCEPTION_UNKNOWN, exception.StatusCode))
            {
                StatusCode = Constants.StatusCodeResApi.Success200
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", exception.Message);
            logData.Properties.Add("ErrorCode", exception.ErrorCode);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }

        private void HandleErrorUnknownException(ExceptionContext context)
        {
            var exception = context.Exception as UnknowException;

            context.Result = new OkObjectResult(new DefaultResponse().Error(ApiConstants.MessageResource.ERROR_500_MESSAGE, exception.StatusCode))
            {
                StatusCode = Constants.StatusCodeResApi.Success200
            };

            var logData = new LogEventInfo(NLog.LogLevel.Error, "", exception.Message);
            logData.Properties.Add("ErrorCode", exception.ErrorCode);
            logData.Exception = context.Exception;

            log.Error(logData);

            context.ExceptionHandled = true;
        }
    }
}
