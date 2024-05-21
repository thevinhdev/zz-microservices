using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Users.ViewModels
{
    public class ResSendOTP
    {
        public string phone { get; set; }
        public string mess { get; set; }
        public string code { get; set; }
        public int? projectId { get; set; }
    }
}
