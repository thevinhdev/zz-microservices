using System.Collections.Generic;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IResult
    {
        bool Successded { get; set; }
        string[] Errors { get; set; }
        IResult Success();
        IResult Failure(List<string> errors);
    }
}
