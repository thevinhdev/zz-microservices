using FluentValidation.Results;
using IOIT.Shared.Commons.Constants;
using System.Collections.Generic;
using System.Linq;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class ValidationException : GenericException
    {
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        public ValidationException(string message, System.Exception innerException = null)
             : base(message, innerException)
        { }
        public ValidationException(string key, string message, string errorCode = ApiConstants.ErrorCode.ERROR_VALIDATION)
            : base()
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(key, message) };

            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
            Message = failures.FirstOrDefault().ErrorMessage;
            ErrorCode = errorCode;
        }
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
            Message = failures.FirstOrDefault().ErrorMessage;
            ErrorCode = ApiConstants.ErrorCode.ERROR_VALIDATION;
        }

    }
}
