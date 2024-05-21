using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IOIT.Identity.Domain.Enum;

namespace IOIT.Identity.Application.Utilities.ViewModels
{
    public class ResGetUtilitiesById
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DomainEnum.TypeUtilities Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<int> ListProjectId { get; set; }
    }
}
