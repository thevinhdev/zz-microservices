using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.BackgroundJobs.Extensions
{
    public class JobConfig
    {
        public string Cron { get; set; }
        public bool Enabled { get; set; }
    }
}
