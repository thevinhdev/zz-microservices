using IOIT.Shared.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }

        public BadRequestException()
            : base()
        {
            Message = string.Empty;
            StatusCode = Constants.StatusCodeResApi.Error400;
            ErrorCode = ApiConstants.ErrorCode.ERROR_BADREQUEST;
        }

        public BadRequestException(string message = "")
            : base(message)
        {
            Message = message;
            StatusCode = Constants.StatusCodeResApi.Error400;
            ErrorCode = ApiConstants.ErrorCode.ERROR_BADREQUEST;
        }

        public BadRequestException(string message = "", string errorCode = ApiConstants.ErrorCode.ERROR_BADREQUEST)
            : base(message)
        {
            Message = message;
            StatusCode = Constants.StatusCodeResApi.Error400;
            ErrorCode = errorCode;
        }

        public BadRequestException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error400)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = ApiConstants.ErrorCode.ERROR_BADREQUEST;
        }

        public BadRequestException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error400, string errorCode = ApiConstants.ErrorCode.ERROR_BADREQUEST)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = errorCode;
        }

        public BadRequestException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error400, string errorCode = ApiConstants.ErrorCode.ERROR_BADREQUEST, Exception innerException = null)
            : base(message, innerException)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = errorCode;
        }

        public BadRequestException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
