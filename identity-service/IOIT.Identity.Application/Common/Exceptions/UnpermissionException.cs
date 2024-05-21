using IOIT.Shared.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class UnpermissionException : Exception
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }

        public UnpermissionException()
            : base()
        {
            Message = string.Empty;
            StatusCode = ApiConstants.StatusCode.NoPermision;
            ErrorCode = ApiConstants.ErrorCode.ERROR_UNPERMISSION;
        }

        public UnpermissionException(string message = "")
            : base(message)
        {
            Message = message;
            StatusCode = ApiConstants.StatusCode.NoPermision;
            ErrorCode = ApiConstants.ErrorCode.ERROR_UNPERMISSION;
        }

        public UnpermissionException(string message = "", int errorStatus = ApiConstants.StatusCode.NoPermision)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = ApiConstants.ErrorCode.ERROR_UNPERMISSION;
        }

        public UnpermissionException(string message = "", int errorStatus = ApiConstants.StatusCode.NoPermision, string errorCode = ApiConstants.ErrorCode.ERROR_UNPERMISSION)
            : base(message)
        {
            Message = message;
            StatusCode = errorStatus;
            ErrorCode = errorCode;
        }
    }
}
