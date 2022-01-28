using System.ComponentModel.DataAnnotations;

namespace AppName.DataCenter.Client.Models
{
    public class PasswordUpdateData
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Old { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string New { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Confirm { get; set; }

        public string Validate()
        {
            if (New.Any(c => char.IsWhiteSpace(c)))
            {
                return "密码不可包含空格";
            }
            if (Old == New)
            {
                return "新旧密码不可相同";
            }
            if (New != Confirm)
            {
                return "新密码输入不一致";
            }
            return default;
        }
    }
}