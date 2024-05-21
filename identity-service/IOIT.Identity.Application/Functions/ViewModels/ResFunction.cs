using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Functions.ViewModels
{
    public class ResFunction
    {
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public int? location { get; set; }
        public bool? selected { get; set; }
        public bool? is_max { get; set; }
        public List<ResFunction> children { get; set; }
    }
}
