using System.ComponentModel.DataAnnotations;

namespace AppName.DataCenter.Shared.Requests
{
    public class AccountLoginRequest
    {
        [Display(Name = "账号")]
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Account { get; set; }

        [Display(Name = "密码")]
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Password { get; set; }
    }
}