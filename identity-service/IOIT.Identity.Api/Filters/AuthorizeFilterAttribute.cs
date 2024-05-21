using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Castle.Core.Internal;
using IOIT.Identity.Api.Models;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace IOIT.Identity.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeFilterAttribute : Attribute, IAuthorizationFilter
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenHeader = context.HttpContext.Request.Headers["Authorization"];

            if (tokenHeader.IsNullOrEmpty())
            {
                var logData = new LogEventInfo(NLog.LogLevel.Error, "", Resources.TOKEN_NULL);
                logData.Properties.Add("ErrorCode", ApiConstants.ErrorCode.ERROR_AUTHORIZED);

                _logger.Error(logData);

                context.Result = new OkObjectResult(
                    new DefaultResponse().Error(
                        Resources.TOKEN_NULL,
                        ApiConstants.StatusCode.Error401))
                {
                    StatusCode = Constants.StatusCodeResApi.Error401
                };
            }

            //var jwtToken = tokenHeader.ToString().Replace("Bearer ", string.Empty);
            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3d0a6b32ed788db9f5598c1188b8f6d0")),
            //    ValidateIssuer = true, //_options.Value.TokenValidationParameter.ValidateIssuer,
            //    ValidateAudience = false,// _options.Value.TokenValidationParameter.ValidateAudience, //you might want to validate the audience and issuer depending on your use case
            //    ValidateLifetime = true,// _options.Value.TokenValidationParameter.ValidateLifetime, //here we are saying that we don't care about the token's expiration date
            //    ValidateIssuerSigningKey = true,// _options.Value.TokenValidationParameter.ValidateIssuerSigningKey,
            //    ValidIssuer = "https://api-pilot.tnsplus.vn/",
            //    ClockSkew = TimeSpan.Zero
            //};

            //try
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken securityToken);
            //    var jwtSecurityToken = securityToken as JwtSecurityToken;

            //    if (jwtSecurityToken == null)
            //    {
            //        context.Result = new OkObjectResult(
            //            new DefaultResponse().Error(
            //                Resources.TOKEN_NULL,
            //                ApiConstants.StatusCode.Error401))
            //        {
            //            StatusCode = Constants.StatusCodeResApi.Error401
            //        };
            //    }
            //    else if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        var logData = new LogEventInfo(NLog.LogLevel.Error, "", Resources.TOKEN_INVALID);
            //        logData.Properties.Add("ErrorCode", ApiConstants.ErrorCode.ERROR_AUTHORIZED);

            //        _logger.Error(logData);

            //        context.Result = new OkObjectResult(
            //            new DefaultResponse().Error(
            //                Resources.TOKEN_INVALID,
            //                ApiConstants.StatusCode.Error401))
            //        {
            //            StatusCode = Constants.StatusCodeResApi.Error401
            //        };
            //    }
            //}
            //catch (Exception e)
            //{
            //    var logData = new LogEventInfo(NLog.LogLevel.Error, "", e.Message);
            //    logData.Properties.Add("ErrorCode", ApiConstants.ErrorCode.ERROR_AUTHORIZED);

            //    _logger.Error(logData);

            //    context.Result = new OkObjectResult(
            //        new DefaultResponse().Error(
            //            Resources.TOKEN_INVALID,
            //            ApiConstants.StatusCode.Error401))
            //    {
            //        StatusCode = Constants.StatusCodeResApi.Error401
            //    };
            //}
        }
    }
}
