using System.ComponentModel.DataAnnotations;

namespace AppName.DataCenter.Shared.Requests
{
    public class ModNamePageListRequest : PageListRequestBase
    {
        [Range(1, int.MaxValue)]
        public int? Id { get; set; }

        public long? CreateTimeMin { get; set; }

        public long? CreateTimeMax { get; set; }
    }
}