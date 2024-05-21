using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.UserRoles.Queries
{
    public class ResGetUserByFunction
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public long EmployeeId { get; set; }
        public string Code { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public bool? IsMain { get; set; }
        public string Note { get; set; }
    }
}
