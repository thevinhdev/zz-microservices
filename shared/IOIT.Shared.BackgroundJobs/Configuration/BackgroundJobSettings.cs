using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.BackgroundJobs.Configuration
{
    public class BackgroundJobSettings
    {
        public string ConnectionString { get; set; }
        public string DashboardEndpoint { get; set; }
        public string TimeZone { get; set; }
    }
}
