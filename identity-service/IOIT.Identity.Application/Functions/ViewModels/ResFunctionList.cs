using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Functions.ViewModels
{
    public class ResFunctionList
    {
        public int FunctionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int FunctionParentId { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? Location { get; set; }
        public string Icon { get; set; }
        public bool? IsParamRoute { get; set; }
        public int? Status { get; set; }
        public int? RowCount { get; set; }

        public ResSmallFunction functionParent { get; set; }
    }

    public class ResFunctionLists
    {
        public List<ResFunctionList> Results { get; set; }
        public int RowCount { get; set; }

    }
}
