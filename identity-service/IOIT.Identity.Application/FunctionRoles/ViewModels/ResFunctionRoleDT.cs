using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.FunctionRoles.ViewModels
{
    public class ResFunctionRoleDT
    {
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string  Note { get; set; }
        public int Status { get; set; }
        public List<ListFunctionDT> listFunction { get; set; }
    }

    public class ListFunctionDT
    {
        public int FunctionId { get; set; }
        public string ActiveKey { get; set; }
    }

    public class ResFunctionRoleLists
    {
        public List<ResFunctionRoleDT> Results { get; set; }
        public int RowCount { get; set; }

    }
}
