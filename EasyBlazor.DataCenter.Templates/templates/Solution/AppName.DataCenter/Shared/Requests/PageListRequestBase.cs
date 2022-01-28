using System.ComponentModel.DataAnnotations;

namespace AppName.DataCenter.Shared.Requests
{
    public class PageListRequestBase
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Index { get; set; }

        [Required]
        [Range(1, 100)]
        public int Size { get; set; }
    }
}