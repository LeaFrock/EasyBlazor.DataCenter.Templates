using System.ComponentModel.DataAnnotations;

namespace AppName.DataCenter.Shared.Requests
{
    public class AccountPasswordRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Old { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string New { get; set; }
    }
}