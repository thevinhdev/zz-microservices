using System.Collections.Generic;
using IOIT.Identity.Domain.Enum;

namespace IOIT.Identity.Application.ProjectUtilities.ViewModels
{
    public class ResGetConfigProjectUtilitiesQuery
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DomainEnum.TypeUtilities Type { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public List<ResGetConfigProjectUtilitiesQuery> ListUtilitiesChild { get; set; }
    }
}
