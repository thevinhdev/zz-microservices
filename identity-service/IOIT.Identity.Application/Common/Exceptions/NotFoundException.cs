using IOIT.Shared.Commons.Constants;
using System;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }

        public NotFoundException()
            : base()
        {
            Message = string.Empty;
            StatusCode = Constants.StatusCodeResApi.Error400;
            ErrorCode = ApiConstants.ErrorCode.ERROR_NOTFOUND;
        }

        public NotFoundException(string message = "")
            : base(message)
        {
            Message = message;
            StatusCode = Constants.StatusCodeResApi.Error400;
            ErrorCode = ApiConstants.ErrorCode.ERROR_NOTFOUND;
        }

        public NotFoundException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error400)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = ApiConstants.ErrorCode.ERROR_NOTFOUND;
        }

        public NotFoundException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error400, string errorCode = ApiConstants.ErrorCode.ERROR_NOTFOUND)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = errorCode;
        }

        public NotFoundException(string message = "", int errorStatus = Constants.StatusCodeResApi.Error400, string errorCode = ApiConstants.ErrorCode.ERROR_NOTFOUND, Exception innerException = null)
            : base(message, innerException)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = errorCode;
        }

        public NotFoundException(string name, object key)
           : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
