using System.ComponentModel.DataAnnotations;

namespace AppName.DataCenter.Shared.Requests
{
    public class AccountInfoUpdateRequest
    {
        [StringLength(16, MinimumLength = 2)]
        public string Name { get; set; }
    }

    public class AccoutInfoCreateRequest : AccountInfoUpdateRequest
    {
        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Password { get; set; }
    }
}