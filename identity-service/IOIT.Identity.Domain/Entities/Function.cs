using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.Entities
{
    public class Function : AbstractEntity
    {
        //public Function()
        //{
        //    FunctionRole = new HashSet<FunctionRole>();
        //}

        //public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int FunctionParentId { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? Location { get; set; }
        public string Icon { get; set; }
        public bool? IsParamRoute { get; set; }

        //public virtual ICollection<FunctionRole> FunctionRole { get; set; }
    }
}
