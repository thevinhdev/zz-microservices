using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Residents.ViewModels
{
    public class DataQuery
    {
        public int? ProjectId { get; set; }
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

    }

    public class ResidentCountry
    {
        public int TotalVn { get; set; }
        public int TotalNn { get; set; }

    }
}
