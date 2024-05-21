using Newtonsoft.Json;

namespace IOIT.Shared.ViewModels.PagingQuery
{
    public class FilterPagination : BasePagination
    {
        public string query { get; set; } = string.Empty;
        public string select { get; set; } = string.Empty;
        public string search { get; set; } = string.Empty;
    }
}
