using System.Collections.Generic;
using IOIT.Identity.Domain.Interfaces;

namespace IOIT.Identity.Application.Models
{
    public class PagedResult<TDataResult> : IPagedResult<TDataResult>
    {
        public List<TDataResult> Results { get; set; } = new List<TDataResult>();
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
