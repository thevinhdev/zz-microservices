using Newtonsoft.Json;

namespace IOIT.Shared.ViewModels.PagingQuery
{
    public class BasePagination
    {
        public int page { get; set; } = 1;
        public int page_size { get; set; } = 100;
        public string order_by { get; set; } = string.Empty;
    }
}
