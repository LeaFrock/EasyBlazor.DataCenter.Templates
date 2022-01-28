using System;
using System.Collections.Generic;

#nullable disable

namespace AppName.DataCenter.Server.Data.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
