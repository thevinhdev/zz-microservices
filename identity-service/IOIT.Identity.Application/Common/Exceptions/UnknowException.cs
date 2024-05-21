using IOIT.Shared.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class UnknowException : Exception
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }

        public UnknowException()
            : base()
        {
            Message = string.Empty;
            StatusCode = Constants.StatusCodeResApi.Error500;
            ErrorCode = ApiConstants.ErrorCode.ERROR_UNKNOWN;
        }

        public UnknowException(string message = "")
            : base(message)
        {
            Message = message;
            StatusCode = Constants.StatusCodeResApi.Error500;
            ErrorCode = ApiConstants.ErrorCode.ERROR_UNKNOWN;
        }

        public UnknowException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error500)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = ApiConstants.ErrorCode.ERROR_UNKNOWN;
        }

        public UnknowException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error500, string errorCode = ApiConstants.ErrorCode.ERROR_UNKNOWN)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = errorCode;
        }
    }
}
