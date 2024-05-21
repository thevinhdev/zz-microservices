using System;
using System.Collections.Generic;
using System.Text;
using IOIT.Identity.Domain.Enum;
using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class Utilities : AbstractEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DomainEnum.TypeUtilities Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
