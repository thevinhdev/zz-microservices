using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.BackgroundJobs.Extensions
{
    public class RecurringJobConfig
    {
        public bool Enabled { get; set; }
        public string Cron { get; set; }
    }

    public class RecurringInvoiceJobConfig
    {
        public bool Enabled { get; set; }
        public string Cron { get; set; }
        public int Maxcheck { get; set; }
        public string ApiGateway { get; set; }
    }
}
